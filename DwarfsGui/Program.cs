namespace DwarfsGui;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        var form = new FrmDwarfs();
        form.ProcessCommandLineArgs(args);
        Application.Run(form);
    }
}