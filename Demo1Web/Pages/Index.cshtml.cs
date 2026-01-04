using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;

namespace Demo1Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        // Properties exposed to the Razor page
        public string MachineName { get; private set; } = string.Empty;
        public string ContainerName { get; private set; } = string.Empty;
        public string OSDescription { get; private set; } = string.Empty;
        public string Framework { get; private set; } = string.Empty;
        public string CurrentTime { get; private set; } = string.Empty;     
        public string OSArchitecture { get; private set; } = string.Empty;
        public string ProcessArchitecture { get; private set; } = string.Empty;
        public string UserName { get; private set; } = string.Empty;
        public string ProcessorCount { get; private set; } = string.Empty;
        public string WorkingSet { get; private set; } = string.Empty;
        public string Uptime { get; private set; } = string.Empty;

        // Client-side libs found in wwwroot
        public List<ClientLib> ClientLibraries { get; private set; } = new();

        // Loaded server-side assemblies
        public List<AssemblyInfo> ServerAssemblies { get; private set; } = new();

        public void OnGet()
        {
            MachineName = Environment.MachineName;
            ContainerName =
                Environment.GetEnvironmentVariable("CONTAINER_NAME")
                ?? Environment.GetEnvironmentVariable("HOSTNAME")
                ?? "Unknown";
            OSDescription = RuntimeInformation.OSDescription;
            Framework = RuntimeInformation.FrameworkDescription;
            OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
            ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
            UserName = Environment.UserName;
            ProcessorCount = Environment.ProcessorCount.ToString();
            WorkingSet = $"{Environment.WorkingSet / (1024 * 1024)} MB";
            CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Uptime via tick count (approximate)
            var uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            Uptime = $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";

            // === Client-side libraries: try to read versions from file headers in wwwroot/lib
            TryAddClientLib("jQuery", "lib/jquery/dist/jquery.min.js");
            TryAddClientLib("Bootstrap CSS", "lib/bootstrap/dist/css/bootstrap.min.css");
            TryAddClientLib("Bootstrap JS", "lib/bootstrap/dist/js/bootstrap.bundle.min.js");

            // === Server-side assemblies: list notable loaded ones
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a =>
                {
                    var an = a.GetName();
                    return new AssemblyInfo
                    {
                        Name = an.Name ?? "Unknown",
                        Version = an.Version?.ToString() ?? "Unknown"
                    };
                })
                // show your app + main ASP.NET Core bits first
                .OrderByDescending(x => x.Name?.StartsWith("Demo1Web") == true)
                .ThenByDescending(x => x.Name?.StartsWith("Microsoft.AspNetCore") == true)
                .ThenBy(x => x.Name)
                .ToList();

            ServerAssemblies = assemblies;
        }

        private void TryAddClientLib(string displayName, string relativePath)
        {
            try
            {
                // WebRootPath points to wwwroot in the published container (/app/wwwroot)
                var full = Path.Combine(_env.WebRootPath ?? "", relativePath.Replace('/', Path.DirectorySeparatorChar));
                if (!System.IO.File.Exists(full))
                {
                    ClientLibraries.Add(new ClientLib
                    {
                        Name = displayName,
                        Version = "Not found",
                        Path = relativePath,
                        SizeBytes = 0
                    });
                    return;
                }

                var bytes = System.IO.File.ReadAllBytes(full);
                var head = Encoding.UTF8.GetString(bytes, 0, Math.Min(bytes.Length, 4096)); // read header only
                var version = ExtractVersion(head) ?? "Unknown";
                var fi = new FileInfo(full);

                ClientLibraries.Add(new ClientLib
                {
                    Name = displayName,
                    Version = version,
                    Path = relativePath,
                    SizeBytes = fi.Length
                });
            }
            catch
            {
                ClientLibraries.Add(new ClientLib
                {
                    Name = displayName,
                    Version = "Error",
                    Path = relativePath,
                    SizeBytes = 0
                });
            }
        }

        // Try common patterns like:
        // /*! jQuery v3.7.1 | ... */
        // /*! Bootstrap v5.3.3 | ... */
        private static string? ExtractVersion(string headerText)
        {
            var rx = new Regex(@"\bv(\d+\.\d+\.\d+)\b", RegexOptions.IgnoreCase);
            var m = rx.Match(headerText);
            if (m.Success) return m.Groups[1].Value;

            // fallback: x.y or x.y.z anywhere
            var rx2 = new Regex(@"\b(\d+\.\d+(?:\.\d+)?)\b");
            var m2 = rx2.Match(headerText);
            if (m2.Success) return m2.Groups[1].Value;

            return null;
        }


        public class ClientLib
        {
            public string Name { get; set; } = "";
            public string Version { get; set; } = "";
            public string Path { get; set; } = "";
            public long SizeBytes { get; set; }
        }

        public class AssemblyInfo
        {
            public string Name { get; set; } = "";
            public string Version { get; set; } = "";
        }
    }
}
