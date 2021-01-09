using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace God2Iso {
    static class Program {

        private static Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolv);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static Assembly AssemblyResolv(object sender, ResolveEventArgs args) {
            if (assemblies.ContainsKey(args.Name)) {
                return assemblies[args.Name];
            } else {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                string[] resources = executingAssembly.GetManifestResourceNames();
                foreach (string resource in resources) {
                    if (resource.EndsWith(".dll")) {
                        Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(resource);
                        byte[] buffer = new byte[manifestResourceStream.Length];
                        manifestResourceStream.Read(buffer, 0, buffer.Length);
                        try {
                            Assembly assembly = Assembly.Load(buffer);
                            if (assembly.FullName.Equals(args.Name)) {
                                assemblies.Add(assembly.FullName, assembly);
                                return assembly;
                            }
                        } catch (Exception) { }
                    }
                }
            }
            return null;
        }
    }
}