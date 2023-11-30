Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Imports System.IO.Compression
Imports System.ServiceProcess

Module Module1
    Dim notifyIcon As NotifyIcon

    Private WithEvents fileSystemWatcher As FileSystemWatcher

    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Create a NotifyIcon
        notifyIcon = New NotifyIcon()
        notifyIcon.Icon = SystemIcons.Application
        notifyIcon.Text = "Your Console App"

        ' Create a context menu for the NotifyIcon
        Dim contextMenu As New ContextMenuStrip()
        contextMenu.Items.Add("Open", Nothing, AddressOf OpenMenuItem_Click)
        contextMenu.Items.Add("Exit", Nothing, AddressOf ExitMenuItem_Click)

        ' Assign the context menu to the NotifyIcon
        notifyIcon.ContextMenuStrip = contextMenu

        ' Handle the FormClosing event to minimize the form to the taskbar
        AddHandler Application.ApplicationExit, AddressOf ApplicationExit

        ' Show the NotifyIcon
        notifyIcon.Visible = True

#Region "Begin filewatcher"
        ' Set the path to the Downloads folder
        Dim downloadsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads"

        ' Create and configure the FileSystemWatcher
        fileSystemWatcher = New FileSystemWatcher(downloadsPath)
        fileSystemWatcher.Filter = "*.zip"
        fileSystemWatcher.EnableRaisingEvents = True

        ' Attach event handlers
        AddHandler fileSystemWatcher.Created, AddressOf OnFileCreated
#End Region

        ' The console application logic should run in a loop or wait for events
        ' For simplicity, we'll just wait for user input here
        Console.WriteLine("Press Enter to exit...")
        Console.ReadLine()

        ' Clean up and exit
        notifyIcon.Visible = False
    End Sub
    Private Sub OnFileCreated(sender As Object, e As FileSystemEventArgs)
        ' Handle the file creation event
        Dim zipFilePath As String = e.FullPath

        ' Create a folder with the same name as the zip file (without extension)
        Dim destinationFolder As String = Path.Combine(Path.GetDirectoryName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath))

        Try
            ' Create the destination folder
            Directory.CreateDirectory(destinationFolder)

            ' Unzip the contents of the zip file into the destination folder
            ZipFile.ExtractToDirectory(zipFilePath, destinationFolder)

            ' Add your additional processing logic here if needed
            Process.Start(destinationFolder)

            ' For example, you might want to log the successful extraction or perform other actions
            ' You can use the destinationFolder variable to reference the folder where the contents were extracted.

        Catch ex As Exception
            ' Handle any exceptions that may occur during the extraction process
            ' Log or display an error message as needed
        End Try
    End Sub


    Private Sub OpenMenuItem_Click(sender As Object, e As EventArgs)
        ' Add code to open your application or perform any desired action
        Console.WriteLine("Open clicked")
    End Sub

    Private Sub ExitMenuItem_Click(sender As Object, e As EventArgs)
        ' Add code to gracefully exit your application
        ' Stop monitoring the folder
        fileSystemWatcher.EnableRaisingEvents = False

        ' Detach event handlers
        RemoveHandler fileSystemWatcher.Created, AddressOf OnFileCreated

        ' Dispose of the FileSystemWatcher
        fileSystemWatcher.Dispose()
        Application.Exit()
    End Sub

    Private Sub ApplicationExit(sender As Object, e As EventArgs)
        ' Clean up and exit
        notifyIcon.Visible = False
    End Sub
End Module
