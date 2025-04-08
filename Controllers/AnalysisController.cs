
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using AIAcademy.Model;
using Razorpay.Api.Errors;
using System.IO;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
namespace AIAcademy.Controllers
{

    public class CodeAnalysisRequest
    {
        [Required]
        public string Code { get; set; }
    }

    [ApiController]
    [Route("api/analysis")]
    [Produces("application/json")]
    public class AnalysisController : ControllerBase
    {


        private  readonly IWebHostEnvironment _env; 

        public AnalysisController(IWebHostEnvironment env)
        {
            _env = env;
            
           
        }

        private bool IsUserCodeError(
    Diagnostic diagnostic,
    SyntaxTree userTree,
    SemanticModel semanticModel)
        {
            // 1. Only C# compiler errors in user's syntax tree
            if (!diagnostic.Id.StartsWith("CS") ||
                !diagnostic.Location.IsInSource ||
                diagnostic.Location.SourceTree != userTree)
                return false;

            // 2. Skip framework types/attributes
            var node = diagnostic.Location.SourceTree
                .GetRoot()
                .FindNode(diagnostic.Location.SourceSpan);

            if (node == null)
                return false;

            var symbol = semanticModel.GetSymbolInfo(node).Symbol;
            if (symbol?.ContainingAssembly?.Name?.StartsWith("System.") == true)
                return false;

            // 3. Skip attribute-related diagnostics
            return node.Parent is not AttributeSyntax;
        }

        [HttpPost]
        [Route("csharp")]
        [RequestSizeLimit(10_000_000)] // 10MB limit
        public  ActionResult<List<CodeError>> AnalyzeCSharpCode([FromBody] CodeAnalysisRequest submission)
        {

            return  AnalyzeCode(submission.Code);     
        }
          List<CodeError> AnalyzeCode(string code)
        {
            var errors = new List<CodeError>();
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var compilation = CreateCompilation(syntaxTree);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            // Get compiler errors
            var diagnostics = compilation.GetDiagnostics()
          .Where(d => d.Severity == DiagnosticSeverity.Error)
          .Where(d => IsUserCodeError(d, syntaxTree, semanticModel));

           //string[] lines = System.IO.File.ReadAllLines(Path.Combine(_env.ContentRootPath, "Dataset", "error_type_suggestions.csv"));

            //string[] realLines = System.IO.File.ReadAllLines(Path.Combine(_env.ContentRootPath, "Dataset", "csharp_error.txt"));

            string[] rosLynerrorLines = System.IO.File.ReadAllLines(Path.Combine(_env.ContentRootPath,"Dataset", "Error_Codes.txt"));
           

            foreach (var diagnostic in diagnostics)
            {
                var lineSpan = diagnostic.Location.GetLineSpan();
                string errorCode = diagnostic.Id;
                string errorMessage = diagnostic.GetMessage();
                int lineNumber = lineSpan.StartLinePosition.Line + 1;
                string errorType = GetErrorType(errorCode);
                string affectedCode = GetAffectedCode(diagnostic.Location, code);

                var syntaxTree1 = diagnostic.Location.SourceTree;
                var syntaxNode = syntaxTree1?.GetRoot().FindNode(diagnostic.Location.SourceSpan);

                string kind = " ";
               
                if (syntaxNode != null)
                {
                    // Skip diagnostics from .NET attributes
                    if (syntaxNode.Parent is AttributeSyntax)
                        continue;

                    // Skip diagnostics from framework types
                    var symbol = compilation.GetSemanticModel(syntaxTree)
                                           .GetSymbolInfo(syntaxNode).Symbol;

                    if (symbol?.ContainingAssembly?.Name?.StartsWith("System.") == true)
                        continue;
                }
                string suggestions = " ", weakerAreas = " ";
                StringBuilder stringBuildersuggestion = new StringBuilder();
                foreach (var item in rosLynerrorLines)
                {
                    string[] data = item.Split(',');
                    if (data[0].Contains(errorCode))
                    {

                        for (int i = 1; i <= data.Length-1; i++)
                        {
                            if (i == data.Length - 2)
                            {
                                stringBuildersuggestion.Append(".");
                            }
                            stringBuildersuggestion.Append(data[i]);
                           
                        }
                      
                        weakerAreas = data[data.Length-1];
                        continue;
                    }
                }

                //string suggestion = " ";
                //foreach (var item in realLines)
                //{
                //    if (item.Contains(errorCode))
                //    {
                //        suggestion = item;
                //        continue;
                //    }
                //}
               
                
               

                errors.Add(new CodeError
                {
                    ErrorCode = errorCode,
                    Message = errorMessage,
                    LineNumber = lineNumber,
                    ErrorType = errorType,
                    AffectedCode = affectedCode,
                    Kind = kind,
                    SuggestionsByTrainer = stringBuildersuggestion.ToString(),
                    WeakAreasOfCSharp = weakerAreas
                }) ; ;
            }

            return errors;
        }

        static CSharpCompilation CreateCompilation(SyntaxTree syntaxTree)
        {
            // Only include basic references needed for compilation
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            return CSharpCompilation.Create("ErrorAnalysis")
                .AddReferences(references)
                .AddSyntaxTrees(syntaxTree)
                .WithOptions(new CSharpCompilationOptions(
    OutputKind.DynamicallyLinkedLibrary,
    generalDiagnosticOption: ReportDiagnostic.Suppress, // Ignore most warnings
    specificDiagnosticOptions: new Dictionary<string, ReportDiagnostic> {
        // Explicitly enable ONLY CS errors in user code
        { "CS", ReportDiagnostic.Error }
    }
));
        }

        static string GetErrorType(string errorCode)
        {
            // Categorize errors by C# version/feature
            return errorCode switch
            {
                var x when x.StartsWith("CS") => "Compiler Error",
                var x when x.StartsWith("IDE") => "IDE Suggestion",
                _ => "Other Issue"
            };
        }

        static string GetAffectedCode(Location location, string code)
        {
            if (!location.IsInSource) return "N/A";

            var textSpan = location.SourceSpan;
            return code.Substring(textSpan.Start,
                Math.Min(50, textSpan.Length)); // Show first 50 chars
        }

        static void DisplayResults(List<CodeError> errors)
        {
            if (!errors.Any())
            {
                Console.WriteLine("\nNo errors detected!");
                return;
            }

            Console.WriteLine("\nDetected Errors:");
            Console.WriteLine("================");

            foreach (var error in errors)
            {
                Console.WriteLine($"\nError: {error.ErrorCode} ({error.ErrorType})");
                Console.WriteLine($"Line {error.LineNumber}: {error.Message}");
                Console.WriteLine($"Affected Code: {error.AffectedCode}");

                Console.WriteLine($"Kind : {error.Kind}");

                // Add specific suggestions
                Console.WriteLine($"Suggestion: {GetSuggestion(error.ErrorCode)}");

                // Console.WriteLine($"{error.ErrorCode} ({error.ErrorType}){error.LineNumber}: {error.Message}{error.AffectedCode}");
            }
        }

        static string GetSuggestion(string Kind)
        {
            return " ";
            // Provide simple suggestions for common errors
            //return errorCode switch
            //{
            //    "CS1002" => "Missing semicolon (;) at end of statement",
            //    "CS0103" => "Variable name doesn't exist in current context",
            //    "CS0165" => "Variable used before being assigned a value",
            //    "CS1061" => "Member doesn't exist on type - check spelling",
            //    "CS0246" => "Type or namespace not found - missing using directive?",
            //    "CS1579" => "Cannot use foreach on this type - needs IEnumerable",
            //    "CS1503" => "Argument type mismatch - check method parameters",
            //    "CS7036" => "Missing required argument - check method signature",
            //    "CS0029" => "Cannot implicitly convert types - explicit cast needed",
            //    "CS0162" => "Unreachable code detected - check logic flow",
            //    "CS0219" => "Variable is assigned but never used - consider removing",
            //    "CS8600" => "Possible null reference - add null check (C# 8.0 nullable)",
            //    "CS8618" => "Non-nullable field not initialized - add constructor initialization",
            //    _ => "See Microsoft documentation for this error"
            //};
        }
    }

}



