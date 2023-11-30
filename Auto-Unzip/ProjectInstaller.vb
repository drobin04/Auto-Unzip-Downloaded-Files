Imports System.ComponentModel
Imports System.ServiceProcess

<RunInstaller(True)>
Public Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    Private serviceProcessInstaller As ServiceProcessInstaller
    Private serviceInstaller As ServiceInstaller

    Public Sub New()
        ' This call is required by the Designer.
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        ' ServiceProcessInstaller
        serviceProcessInstaller = New ServiceProcessInstaller()
        serviceProcessInstaller.Account = ServiceAccount.User ' Set the account type as needed

        ' ServiceInstaller
        serviceInstaller = New ServiceInstaller()
        serviceInstaller.ServiceName = "Auto-Unzip Service"
        serviceInstaller.StartType = ServiceStartMode.Automatic ' Set the start type as needed

        ' Attach installers to the ProjectInstaller
        Installers.AddRange(New System.Configuration.Install.Installer() {serviceProcessInstaller, serviceInstaller})
    End Sub
End Class
