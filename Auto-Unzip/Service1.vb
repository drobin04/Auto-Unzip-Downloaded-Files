Imports System.IO
Imports System.IO.Compression
Imports System.ServiceProcess

Public Class Service1
    Inherits ServiceBase

    Private WithEvents fileSystemWatcher As FileSystemWatcher


    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Set the path to the Downloads folder
        Dim downloadsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads"

        ' Create and configure the FileSystemWatcher
        fileSystemWatcher = New FileSystemWatcher(downloadsPath)
        fileSystemWatcher.Filter = "*.zip"
        fileSystemWatcher.EnableRaisingEvents = True

        ' Attach event handlers
        AddHandler fileSystemWatcher.Created, AddressOf OnFileCreated
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



    Protected Overrides Sub OnStop()
        ' Stop monitoring the folder
        fileSystemWatcher.EnableRaisingEvents = False

        ' Detach event handlers
        RemoveHandler fileSystemWatcher.Created, AddressOf OnFileCreated

        ' Dispose of the FileSystemWatcher
        fileSystemWatcher.Dispose()
    End Sub


End Class
