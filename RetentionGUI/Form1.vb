Imports Microsoft.Win32


Public Class Form1

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label7.Text = Environment.UserName

        Dim PathToMonitor = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "PathToMonitor", Nothing)
        Dim DaysToKeep = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "DaysToKeep", Nothing)
        Dim TimeOfDayToDeleteFiles = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "TimeOfDayToDeleteFiles", Nothing)

        TextBox1.Text = DaysToKeep
        TextBox3.Text = PathToMonitor
        TextBox4.Text = TimeOfDayToDeleteFiles

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        On Error Resume Next
        Dim sc As New System.ServiceProcess.ServiceController("Retention", My.Computer.Name.ToString())
        'sc.ServiceName = "Retention"
        Dim status = sc.Status
        If status = 1 Then Label1.BackColor = Color.Red : StopRetentionServiceToolStripMenuItem.Enabled = False : StartRetentionServiceToolStripMenuItem.Enabled = True
        If status = 4 Then Label1.BackColor = Color.Green : StartRetentionServiceToolStripMenuItem.Enabled = False : StopRetentionServiceToolStripMenuItem.Enabled = True
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub StopRetentionServiceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopRetentionServiceToolStripMenuItem.Click
        Dim sc As New System.ServiceProcess.ServiceController("Retention", My.Computer.Name.ToString())
        sc.ServiceName = "Retention"
        sc.Stop()
    End Sub

    Private Sub StartRetentionServiceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartRetentionServiceToolStripMenuItem.Click
        Dim sc As New System.ServiceProcess.ServiceController("Retention", My.Computer.Name.ToString())
        sc.ServiceName = "Retention"
        sc.Start()
    End Sub

    Private Sub SaveChangesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveChangesToolStripMenuItem.Click
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "DaysToKeep", Convert.ToInt32(TextBox1.Text))
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "PathToMonitor", TextBox3.Text)
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "TimeOfDayToDeleteFiles", TextBox4.Text)

        Dim sc As New System.ServiceProcess.ServiceController("Retention", My.Computer.Name.ToString())
        sc.ServiceName = "Retention"
        sc.Stop()
        System.Threading.Thread.Sleep(1000)
        sc.Start()

    End Sub


    Private Sub CreateRegistryKeysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateRegistryKeysToolStripMenuItem.Click

        My.Computer.Registry.CurrentConfig.CreateSubKey("HKEY_LOCAL_MACHINE\SOFTWARE\Retention")
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "DaysToKeep", Convert.ToInt32("30"))
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "PathToMonitor", "C:\ProgramData\Gallagher\Backup\")
        My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Retention", "TimeOfDayToDeleteFiles", "18:00")

    End Sub

    Private Sub InstallRetentionServiceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InstallRetentionServiceToolStripMenuItem.Click

        Process.Start("cmd", "/c mkdir C:\Retention")
        Dim appPath = Application.ExecutablePath
        MsgBox(appPath)
        Process.Start("cmd", "/c copy " & "Retention.exe C:\Retention\")
        Process.Start("cmd", "/c sc create Retention binpath= C:\Retention\Retention.exe start= auto")

        System.Threading.Thread.Sleep(1000)


        Dim sc As New System.ServiceProcess.ServiceController("Retention", My.Computer.Name.ToString())
        sc.ServiceName = "Retention"
        sc.Start()
        MsgBox("Retention was Installed you may try to start the service")

    End Sub

    Private Sub UninstallRetentionServiceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UninstallRetentionServiceToolStripMenuItem.Click

        Dim sc As New System.ServiceProcess.ServiceController("Retention", My.Computer.Name.ToString())
        sc.ServiceName = "Retention"
        sc.Stop()
        Process.Start("cmd", "/c sc delete Retention")
        MsgBox("The Retention Process has been removed !")

    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Form2.Show()
        Timer2.Enabled = True
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Form2.Hide()
        Timer2.Enabled = False
    End Sub
End Class
