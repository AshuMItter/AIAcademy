using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AIAcademy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TocController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public TocController(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpGet]
        [Route("python-ai")]
        public async Task<IActionResult> GetPythonAITrack()
        {
            var html = GenerateFuturisticHtml(
                "🐍 Create AI with Python and TensorFlow",
                "Master AI development with Python, TensorFlow, and modern ML techniques",
                GetPythonAIContent()
            );

            return await ReturnHtmlFile(html, "Python_AI_Track.html");
        }

        private string GetPythonAIContent()
        {
            return @"
<div class='python-ai-header'>
    <h1>🐍 Create AI with Python and TensorFlow <span class='total-duration'>100 Hours</span></h1>
    <p class='subtitle'>Master AI development with Python, TensorFlow, and modern ML techniques</p>
</div>

<div class='track'>
    <h2>1. Python for AI & Data Science <span class='duration'>20 Hours</span></h2>
    
    <div class='module'>
        <h3>1.1 Python Fundamentals for AI <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Python Syntax & Data Structures</li>
            <li>NumPy for Numerical Computing</li>
            <li>Pandas for Data Manipulation</li>
            <li>Matplotlib & Seaborn for Visualization</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>1.2 Advanced Python for ML <span class='duration'>8 Hours</span></h3>
        <ul>
            <li>Object-Oriented Programming in Python</li>
            <li>Functional Programming Techniques</li>
            <li>Parallel Processing & Multiprocessing</li>
            <li>Decorators & Generators for ML Pipelines</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>1.3 Data Preprocessing Techniques <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Feature Engineering & Selection</li>
            <li>Handling Missing Data & Outliers</li>
            <li>Data Normalization & Standardization</li>
            <li>Working with Time Series Data</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>2. Machine Learning Foundations <span class='duration'>24 Hours</span></h2>
    
    <div class='module'>
        <h3>2.1 Supervised Learning <span class='duration'>8 Hours</span></h3>
        <ul>
            <li>Linear & Logistic Regression</li>
            <li>Decision Trees & Random Forests</li>
            <li>Support Vector Machines</li>
            <li>Model Evaluation Metrics</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>2.2 Unsupervised Learning <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Clustering Algorithms (K-Means, DBSCAN)</li>
            <li>Dimensionality Reduction (PCA, t-SNE)</li>
            <li>Anomaly Detection</li>
            <li>Association Rule Learning</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>2.3 Ensemble Methods & Model Optimization <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Bagging & Boosting Techniques</li>
            <li>Hyperparameter Tuning</li>
            <li>Cross-Validation Strategies</li>
            <li>Model Interpretability (SHAP, LIME)</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>2.4 ML Deployment with Python <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Model Serialization (Pickle, Joblib)</li>
            <li>Building REST APIs with Flask/FastAPI</li>
            <li>Containerization with Docker</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>3. Deep Learning with TensorFlow <span class='duration'>30 Hours</span></h2>
    
    <div class='module'>
        <h3>3.1 Neural Network Fundamentals <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Perceptrons & Activation Functions</li>
            <li>Backpropagation & Gradient Descent</li>
            <li>TensorFlow Architecture & Eager Execution</li>
            <li>Building Your First Neural Network</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.2 Computer Vision with CNNs <span class='duration'>8 Hours</span></h3>
        <ul>
            <li>Convolutional Neural Networks</li>
            <li>Transfer Learning with Pre-trained Models</li>
            <li>Image Classification & Object Detection</li>
            <li>Data Augmentation Techniques</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.3 Natural Language Processing <span class='duration'>8 Hours</span></h3>
        <ul>
            <li>Text Preprocessing & Word Embeddings</li>
            <li>Recurrent Neural Networks (RNNs)</li>
            <li>Transformers & Attention Mechanisms</li>
            <li>Building Chatbots & Text Generators</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.4 Advanced Architectures <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Autoencoders & GANs</li>
            <li>Reinforcement Learning Basics</li>
            <li>Time Series Forecasting with LSTMs</li>
            <li>Model Optimization & Quantization</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.5 TensorFlow Deployment <span class='duration'>2 Hours</span></h3>
        <ul>
            <li>Serving Models with TF Serving</li>
            <li>Converting to TensorFlow Lite</li>
            <li>Edge Deployment Options</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>4. Specialized AI Applications <span class='duration'>16 Hours</span></h2>
    
    <div class='module'>
        <h3>4.1 Generative AI <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Text-to-Image Generation</li>
            <li>Diffusion Models</li>
            <li>Ethical Considerations</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>4.2 AI for Healthcare <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Medical Image Analysis</li>
            <li>Predictive Diagnostics</li>
            <li>Privacy & Compliance</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>4.3 AI for Finance <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Algorithmic Trading Models</li>
            <li>Fraud Detection</li>
            <li>Risk Assessment</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>4.4 AI for IoT & Edge Devices <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>TensorFlow Lite for Microcontrollers</li>
            <li>On-Device Machine Learning</li>
            <li>Optimizing for Resource Constraints</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>5. MLOps & Production AI <span class='duration'>10 Hours</span></h2>
    
    <div class='module'>
        <h3>5.1 ML Pipelines <span class='duration'>3 Hours</span></h3>
        <ul>
            <li>Data Versioning (DVC)</li>
            <li>Feature Stores</li>
            <li>Automated Training Pipelines</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>5.2 Model Monitoring & Maintenance <span class='duration'>3 Hours</span></h3>
        <ul>
            <li>Model Drift Detection</li>
            <li>Performance Monitoring</li>
            <li>A/B Testing Models</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>5.3 Scaling AI Systems <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Distributed Training</li>
            <li>Kubernetes for ML</li>
            <li>Cost Optimization</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>🎯 Capstone Project <span class='duration'>20 Hours</span></h2>
    
    <div class='module'>
        <h3>End-to-End AI Solution</h3>
        <ul>
            <li>Problem Definition & Data Collection</li>
            <li>Model Development & Training</li>
            <li>Deployment & User Interface</li>
            <li>Presentation & Documentation</li>
        </ul>
    </div>
</div>

<div class='track summary-track'>
    <h2>📅 Total Program Duration</h2>
    
    <div class='module'>
        <h3>Complete Learning Journey</h3>
        <ul class='duration-summary'>
            <li><span class='summary-label'>Core Curriculum:</span> <span class='summary-hours'>100 Hours</span></li>
            <li><span class='summary-label'>Capstone Project:</span> <span class='summary-hours'>20 Hours</span></li>
            <li class='total-hours'><span class='summary-label'>Total Investment:</span> <span class='summary-hours'>120 Hours</span></li>
        </ul>
    </div>
</div>";
        }

        [HttpGet]
        [Route("productivity")]
        public async Task<IActionResult> GetProductivityTrack()
        {
            var html = GenerateFuturisticHtml(
                "🚀 AI Productivity Track",
                "Master AI-powered tools to supercharge your workflow",
                GetProductivityContent()
            );

            return await ReturnHtmlFile(html, "AI_Productivity_Track.html");
        }

        [HttpGet]
        [Route("development")]
        public async Task<IActionResult> GetDevelopmentTrack()
        {
            var html = GenerateFuturisticHtml(
                "🤖 AI Development Track",
                "Build AI-powered applications with C#, .NET, and ML frameworks",
                GetDevelopmentContent()
            );

            return await ReturnHtmlFile(html, "AI_Development_Track.html");
        }

        private string GenerateFuturisticHtml(string title, string subtitle, string content)
        {
            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
    <style>
        :root {{
            --primary: #0ff0fc;
            --secondary: #ff2a6d;
            --bg: #050517;
            --text: #d1f7ff;
            --glow: 0 0 10px var(--primary), 0 0 20px var(--secondary);
        }}
        
        body {{
            background: var(--bg);
            color: var(--text);
            font-family: 'Segoe UI', system-ui, sans-serif;
            line-height: 1.6;
            padding: 2rem;
            overflow-x: hidden;
        }}
        
        h1, h2, h3 {{
            text-transform: uppercase;
            letter-spacing: 2px;
            animation: pulse 4s infinite alternate;
        }}
        
        h1 {{
            font-size: 2.5rem;
            color: var(--primary);
            text-shadow: var(--glow);
            border-bottom: 2px solid var(--secondary);
            padding-bottom: 1rem;
            margin-bottom: 1rem;
        }}
        
        .subtitle {{
            font-style: italic;
            margin-bottom: 2rem;
            opacity: 0.9;
        }}
        
        h2 {{
            color: var(--secondary);
            margin-top: 2rem;
            position: relative;
            display: inline-block;
        }}
        
        h2::after {{
            content: '';
            position: absolute;
            width: 100%;
            height: 3px;
            background: linear-gradient(90deg, var(--primary), var(--secondary));
            bottom: -5px;
            left: 0;
            transform: scaleX(0);
            transform-origin: left;
            animation: expand 1.5s forwards;
        }}
        
        .track {{
            background: rgba(5, 5, 23, 0.7);
            border: 1px solid rgba(0, 255, 252, 0.2);
            border-radius: 8px;
            padding: 1.5rem;
            margin-bottom: 2rem;
            box-shadow: 0 0 15px rgba(0, 255, 252, 0.1);
            transition: transform 0.3s, box-shadow 0.3s;
        }}
        
        .track:hover {{
            transform: translateY(-5px);
            box-shadow: 0 10px 25px rgba(0, 255, 252, 0.2);
        }}
        
        .module {{
            margin-left: 1rem;
            padding-left: 1rem;
            border-left: 2px dashed var(--secondary);
            animation: fadeIn 1s forwards;
            opacity: 0;
        }}
        
        .module:nth-child(2) {{ animation-delay: 0.3s; }}
        .module:nth-child(3) {{ animation-delay: 0.6s; }}
        .module:nth-child(4) {{ animation-delay: 0.9s; }}
        
        .duration {{
            display: inline-block;
            background: var(--secondary);
            color: white;
            padding: 0.2rem 0.5rem;
            border-radius: 4px;
            font-size: 0.8rem;
            margin-left: 0.5rem;
            animation: blink 2s infinite;
        }}
        
        ul {{
            list-style-type: none;
            padding-left: 1rem;
        }}
        
        li {{
            position: relative;
            padding-left: 1.5rem;
            margin-bottom: 0.5rem;
        }}
        
        li:before {{
            content: '▹';
            position: absolute;
            left: 0;
            color: var(--primary);
        }}
        
        @keyframes pulse {{
            0% {{ opacity: 0.9; }}
            100% {{ opacity: 1; text-shadow: 0 0 15px var(--primary), 0 0 30px var(--secondary); }}
        }}
        
        @keyframes expand {{
            to {{ transform: scaleX(1); }}
        }}
        
        @keyframes fadeIn {{
            to {{ opacity: 1; }}
        }}
        
        @keyframes blink {{
            0%, 100% {{ opacity: 1; }}
            50% {{ opacity: 0.7; }}
        }}
        
        .particles {{
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: -1;
            pointer-events: none;
        }}
        
        .total-duration {{
            background: rgba(255, 42, 109, 0.2);
            padding: 0.5rem 1rem;
            border-radius: 4px;
            margin: 1rem 0;
            display: inline-block;
        }}
    </style>
</head>
<body>
    <div class='particles' id='particles-js'></div>
    <h1>{title}</h1>
    <div class='subtitle'>{subtitle}</div>
    <div class='total-duration'>Total Duration: {GetTotalDuration(title)} Hours</div>
    
    {content}
    
    <script>
        // Animated particles background
        document.addEventListener('DOMContentLoaded', function() {{
            const canvas = document.createElement('canvas');
            canvas.style.position = 'absolute';
            canvas.style.width = '100%';
            canvas.style.height = '100%';
            document.getElementById('particles-js').appendChild(canvas);
            
            const ctx = canvas.getContext('2d');
            canvas.width = canvas.offsetWidth;
            canvas.height = canvas.offsetHeight;
            
            const particles = [];
            const particleCount = window.innerWidth < 768 ? 30 : 100;
            
            for (let i = 0; i < particleCount; i++) {{
                particles.push({{
                    x: Math.random() * canvas.width,
                    y: Math.random() * canvas.height,
                    size: Math.random() * 3 + 1,
                    speedX: Math.random() * 1 - 0.5,
                    speedY: Math.random() * 1 - 0.5,
                    color: `rgba(${{Math.floor(Math.random() * 100 + 155)}}, ${{Math.floor(Math.random() * 100 + 155)}}, 255, ${{Math.random() * 0.5 + 0.2}})`
                }});
            }}
            
            function animate() {{
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                
                for (let i = 0; i < particles.length; i++) {{
                    const p = particles[i];
                    
                    p.x += p.speedX;
                    p.y += p.speedY;
                    
                    if (p.x < 0 || p.x > canvas.width) p.speedX *= -1;
                    if (p.y < 0 || p.y > canvas.height) p.speedY *= -1;
                    
                    ctx.beginPath();
                    ctx.arc(p.x, p.y, p.size, 0, Math.PI * 2);
                    ctx.fillStyle = p.color;
                    ctx.fill();
                    
                    for (let j = i + 1; j < particles.length; j++) {{
                        const p2 = particles[j];
                        const dist = Math.sqrt(Math.pow(p.x - p2.x, 2) + Math.pow(p.y - p2.y, 2));
                        
                        if (dist < 100) {{
                            ctx.beginPath();
                            ctx.strokeStyle = `rgba(0, 255, 252, ${{1 - dist/100}})`;
                            ctx.lineWidth = 0.5;
                            ctx.moveTo(p.x, p.y);
                            ctx.lineTo(p2.x, p2.y);
                            ctx.stroke();
                        }}
                    }}
                }}
                
                requestAnimationFrame(animate);
            }}
            
            animate();
            
            window.addEventListener('resize', function() {{
                canvas.width = canvas.offsetWidth;
                canvas.height = canvas.offsetHeight;
            }});
        }});
        
        // Interactive module highlighting
        document.querySelectorAll('.module').forEach(module => {{
            module.addEventListener('mouseenter', function() {{
                this.style.transform = 'translateX(10px)';
                this.style.borderLeftColor = '#0ff0fc';
            }});
            
            module.addEventListener('mouseleave', function() {{
                this.style.transform = '';
                this.style.borderLeftColor = 'var(--secondary)';
            }});
        }});
        
        // Dynamic color shifting for headings
        setInterval(() => {{
            const headings = document.querySelectorAll('h1, h2, h3');
            headings.forEach(heading => {{
                const hue = Math.floor(Math.random() * 60 + 180);
                heading.style.color = `hsl(${{hue}}, 100%, 70%)`;
            }});
        }}, 3000);
    </script>
</body>
</html>";
        }

        private string GetProductivityContent()
        {
            return @"
   <div class=""productivity-header"">
    <h1>🚀 AI Productivity Track <span class=""total-duration"">45 Hours</span></h1>
    <p class=""subtitle"">Master AI-powered tools to supercharge your workflow</p>
</div>

<div class='track'>
    <h2>1. Prompt Engineering Mastery <span class='duration'>6 Hours</span></h2>
    
    <div class='module'>
        <h3>1.1 Foundations of AI Prompts <span class='duration'>1.5 Hours</span></h3>
        <ul>
            <li>Understanding LLMs (GPT-4, Claude, Gemini)</li>
            <li>Principles of Effective Prompting</li>
            <li>Zero-shot vs. Few-shot Learning</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>1.2 Advanced Prompt Techniques <span class='duration'>2 Hours</span></h3>
        <ul>
            <li>Chain-of-Thought Prompting</li>
            <li>Role-Based Prompting</li>
            <li>Multi-step Reasoning & Iterative Refinement</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>1.3 Real-World Applications <span class='duration'>2.5 Hours</span></h3>
        <ul>
            <li>Automating Business Reports</li>
            <li>AI-Powered Research & Summarization</li>
            <li>Debugging & Optimizing Prompts</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>2. AI for Microsoft Office Suite <span class='duration'>12 Hours</span></h2>
    
    <div class='module'>
        <h3>2.1 AI-Powered Excel Mastery <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Formula Generation with AI</li>
            <li>Data Analysis & Forecasting with AI</li>
            <li>Automating Reports with AI Plugins</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>2.2 AI-Driven Word & Docs <span class='duration'>3 Hours</span></h3>
        <ul>
            <li>Smart Document Summarization</li>
            <li>AI-Based Legal & Technical Writing</li>
            <li>Auto-Formatting & Styling</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>2.3 Next-Gen Presentations with AI <span class='duration'>3 Hours</span></h3>
        <ul>
            <li>AI-Generated Slide Decks</li>
            <li>Dynamic Data Visualization</li>
            <li>Voice-Controlled PPT Edits</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>2.4 AI Grammar & Style Tools <span class='duration'>2 Hours</span></h3>
        <ul>
            <li>Advanced Grammar Correction</li>
            <li>Tone & Style Adaptation</li>
            <li>Plagiarism & Readability Analysis</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>3. AI for Media Generation & Manipulation <span class='duration'>15 Hours</span></h2>
    
    <div class='module'>
        <h3>3.1 AI Image Generation <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>MidJourney, DALL·E, Stable Diffusion</li>
            <li>Text-to-Image Optimization</li>
            <li>Inpainting & Outpainting</li>
            <li>Style Transfer & Hyper-Realism</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.2 AI Video Editing & Generation <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Text-to-Video (Runway, Pika)</li>
            <li>Deepfake & Face Swapping (Ethical Use)</li>
            <li>Automated Subtitles & Voiceovers</li>
            <li>AI-Powered Special Effects</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.3 AI Audio & Voice Synthesis <span class='duration'>3 Hours</span></h3>
        <ul>
            <li>AI Voice Cloning (ElevenLabs)</li>
            <li>Text-to-Speech Customization</li>
            <li>Podcast & Audiobook Automation</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>4. AI Automation & Workflow Optimization <span class='duration'>12 Hours</span></h2>
    
    <div class='module'>
        <h3>4.1 AI-Powered Task Automation <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>No-Code AI Workflows (Zapier, Make)</li>
            <li>AI Chatbots for Customer Support</li>
            <li>Email & Calendar Automation</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>4.2 AI for Data Extraction & Analysis <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>PDF & Document Parsing</li>
            <li>Web Scraping with AI</li>
            <li>AI-Driven Business Intelligence</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>4.3 Future of AI in Productivity <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>AI Agents & Autonomous Workflows</li>
            <li>Self-Learning Office Assistants</li>
            <li>Ethical AI & Bias Mitigation</li>
        </ul>
    </div>
</div>

<div class='track summary-track'>
    <h2>📅 Total Program Duration</h2>
    
    <div class='module'>
        <h3>Complete Learning Journey</h3>
        <ul class='duration-summary'>
            <li><span class='summary-label'>AI Productivity Track:</span> <span class='summary-hours'>45 Hours</span></li>
            <li class='total-hours'><span class='summary-label'>Total Investment:</span> <span class='summary-hours'>45 Hours</span></li>
        </ul>
    </div>
</div>";
        }

        private string GetDevelopmentContent()
        {
            return @"
   <div class=""development-header"">
    <h1>🤖 AI Development Track <span class=""total-duration"">80 Hours</span></h1>
    <p class=""subtitle"">Build AI-powered applications with C#, .NET, and ML frameworks</p>
</div>

<div class='track'>
    <h2>1. C# Programming <span class='duration'>24 Hours</span></h2>
    
    <div class='module'>
        <h3>1.1 C# Fundamentals <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Syntax, Data Types, Control Flow</li>
            <li>OOP (Classes, Inheritance, Polymorphism)</li>
            <li>Error Handling & Debugging</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>1.2 Advanced C# Concepts <span class='duration'>8 Hours</span></h3>
        <ul>
            <li>Delegates, Events, Lambdas</li>
            <li>Async/Await & Multithreading</li>
            <li>Reflection & Dynamic Programming</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>1.3 Modern C# Features <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Records, Pattern Matching</li>
            <li>Source Generators</li>
            <li>High-Performance Code with Span&lt;T&gt;</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>1.4 C# for AI & Cloud <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Integrating AI Libraries</li>
            <li>Serverless C# with Azure Functions</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>2. Database Mastery with SQL & EF Core <span class='duration'>16 Hours</span></h2>
    
    <div class='module'>
        <h3>2.1 SQL for AI Applications <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Advanced Queries (CTEs, Window Functions)</li>
            <li>Optimizing AI Data Pipelines</li>
            <li>Vector Databases for AI</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>2.2 Entity Framework Core <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Code-First Migrations</li>
            <li>Performance Tuning</li>
            <li>EF Core + AI Model Integration</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>2.3 LinQ & Real-Time Data <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Complex Queries with LinQ</li>
            <li>Real-Time AI Data Processing</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>3. .NET AI & Machine Learning <span class='duration'>20 Hours</span></h2>
    
    <div class='module'>
        <h3>3.1 ML.NET Fundamentals <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Regression, Classification, Clustering</li>
            <li>Custom Model Training</li>
            <li>Deploying ML Models in .NET</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.2 Deep Learning with TorchSharp <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Neural Networks in C#</li>
            <li>Image & NLP Models</li>
            <li>Transfer Learning</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.3 AI-Powered Chatbots <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Building LLM-Based Assistants</li>
            <li>RAG (Retrieval-Augmented Generation)</li>
            <li>Voice-Enabled Bots</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>3.4 AI for Media Manipulation <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Image Recognition & Generation</li>
            <li>Video Processing with AI</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>4. Building Scalable AI Solutions <span class='duration'>20 Hours</span></h2>
    
    <div class='module'>
        <h3>4.1 Cloud AI with Azure <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>Cognitive Services Integration</li>
            <li>AI Model Deployment on Cloud</li>
            <li>AutoML for .NET</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>4.2 Edge AI with .NET <span class='duration'>6 Hours</span></h3>
        <ul>
            <li>IoT & AI at the Edge</li>
            <li>ONNX Runtime Optimization</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>4.3 Ethical AI & Future Trends <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>Bias Detection & Fairness</li>
            <li>AI Regulations & Compliance</li>
        </ul>
    </div>
    
    <div class='module'>
        <h3>4.4 Capstone Project <span class='duration'>4 Hours</span></h3>
        <ul>
            <li>End-to-End AI Application</li>
        </ul>
    </div>
</div>

<div class='track'>
    <h2>🎯 Final Certification Project <span class='duration'>10 Hours</span></h2>
    
    <div class='module'>
        <h3>Build & Deploy a Full AI Solution</h3>
        <ul>
            <li>Combining Productivity & Development Tracks</li>
            <li>Real-World AI Automation or AI-Powered App</li>
        </ul>
    </div>
</div>

<div class='track summary-track'>
    <h2>📅 Total Program Duration</h2>
    
    <div class='module'>
        <h3>Complete Learning Journey</h3>
        <ul class='duration-summary'>
            <li><span class='summary-label'>AI Development Track:</span> <span class='summary-hours'>80 Hours</span></li>
            <li><span class='summary-label'>Certification Project:</span> <span class='summary-hours'>10 Hours</span></li>
            <li class='total-hours'><span class='summary-label'>Total Investment:</span> <span class='summary-hours'>90 Hours</span></li>
        </ul>
    </div>
</div>";
        }

        private string GetTotalDuration(string track)
        {
            if(track.ToLower().Contains("productivity"))
            {
                return "45";
            }
            if (track.ToLower().Contains("development"))
            {
                return "80";

            }
            if (track.ToLower().Contains("python"))
            {
                return "100";
            }

            return " ";
        }

        private async Task<IActionResult> ReturnHtmlFile(string html, string filename)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            return File(stream, "text/html", filename);
        }
    }
}