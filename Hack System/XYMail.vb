Public Class XYMail
    Dim NormalColor As Color = Color.FromArgb(19, 132, 205)
    Dim FailColor As Color = Color.FromArgb(248, 97, 97)
    Dim MailThread As Threading.Thread=New Threading.Thread(AddressOf SendMail)

    Private Sub Btn_Send_Click(sender As Object, e As EventArgs) Handles Btn_Send.Click
        If MailThread.ThreadState = Threading.ThreadState.Running Then Exit Sub

        MailThread = New Threading.Thread(AddressOf SendMail)
        Btn_Send.Enabled = False
        ReturnInfo.ForeColor = NormalColor
        ReturnInfo.Text = "Mail Sending ..."
        Application.DoEvents()

        MailThread.Start()
    End Sub

    Private Sub SendMail()
        Try
            Dim Mail As New Net.Mail.MailMessage()
            Dim Smtp As New Net.Mail.SmtpClient("smtp.yeah.net", 25)
            '创建SMTP连接和MAIL对象
            Smtp.UseDefaultCredentials = True
            Smtp.Credentials = New System.Net.NetworkCredential("HackSystem", "HackSystem123") '"I'mHackSystem")
            Mail.From = New System.Net.Mail.MailAddress("HackSystem@yeah.net")
            Mail.To.Add(Txt_ToAddress.Text)
            Mail.SubjectEncoding = System.Text.Encoding.GetEncoding("GB2312")
            Mail.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312")
            Mail.Priority = System.Net.Mail.MailPriority.Normal
            Mail.Subject = Txt_MailTitle.Text
            '邮件内容支持HTML格式
            Mail.IsBodyHtml = True
            'HTML格式需要改变换行符vbCrLf为<br>
            Mail.Body = Replace(Txt_MailBody.Text, vbCrLf, "<br>") & "<br><br><i><small>    ——Form：Hack System (V " & Application.ProductVersion & ")</small></i>"
            '发送邮件
            Smtp.Send(Mail)
            Mail.Dispose()
            ReturnInfo.Text = "Sent Successfully"
        Catch ex As Exception
            '捕获到异常
            Beep()
            ReturnInfo.ForeColor = FailColor
            ReturnInfo.Text = "Failed! Error: " & Err.Number
        End Try

        Btn_Send.Enabled = True
    End Sub

    Private Sub MoveWindow(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        '鼠标通过控件拖动窗体
        WindowsTemplates.ReleaseCapture()
        WindowsTemplates.SendMessageA(Me.Handle, &HA1, 2, 0&)
    End Sub

#Region "发送按钮响应鼠标动态效果"
    Private Sub Btn_Send_MouseDown(sender As Object, e As MouseEventArgs) Handles Btn_Send.MouseDown
        Btn_Send.Image = My.Resources.SystemAssets.SendBtn_2
    End Sub

    Private Sub Btn_Send_MouseEnter(sender As Object, e As EventArgs) Handles Btn_Send.MouseEnter
        Btn_Send.Image = My.Resources.SystemAssets.SendBtn_1
    End Sub

    Private Sub Btn_Send_MouseLeave(sender As Object, e As EventArgs) Handles Btn_Send.MouseLeave
        Btn_Send.Image = My.Resources.SystemAssets.SendBtn_0
    End Sub

    Private Sub Btn_Send_MouseUp(sender As Object, e As MouseEventArgs) Handles Btn_Send.MouseUp
        Btn_Send.Image = My.Resources.SystemAssets.SendBtn_1
    End Sub
#End Region

    Private Sub Txt_ToAddress_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Txt_ToAddress.KeyPress
        '发信人地址栏响应回车键
        If Asc(e.KeyChar) = 13 Then Btn_Send_Click(New Object, New EventArgs)
    End Sub

    Private Sub Btn_Close_Click(sender As Object, e As EventArgs) Handles Btn_Close.Click
        Me.Hide()
    End Sub

    Private Sub XYMail_Load(sender As Object, e As EventArgs) Handles Me.Load
        CheckForIllegalCrossThreadCalls = False
        Txt_ToAddress.SelectionLength = 0
    End Sub

    Private Sub XYMail_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Me.Visible Then
            ReturnInfo.ForeColor = NormalColor
            ReturnInfo.Text = "Mail function is ready."
        Else
            Btn_Send.Enabled = True
            If MailThread.ThreadState = Threading.ThreadState.Running Then
                MailThread.Abort()
                MailThread = Nothing
            End If
        End If
    End Sub


#Region "关闭按钮响应鼠标动态效果"
    Private Sub Btn_Close_MouseEnter(sender As Object, e As EventArgs) Handles Btn_Close.MouseEnter
        Btn_Close.Image = My.Resources.XYBrowserRes.Close_E
    End Sub

    Private Sub Btn_Close_MouseLeave(sender As Object, e As EventArgs) Handles Btn_Close.MouseLeave
        Btn_Close.Image = My.Resources.XYBrowserRes.Close_N
    End Sub

    Private Sub Btn_Close_MouseDown(sender As Object, e As MouseEventArgs) Handles Btn_Close.MouseDown
        Btn_Close.Image = My.Resources.XYBrowserRes.Close_D
    End Sub

    Private Sub Btn_Close_MouseUp(sender As Object, e As MouseEventArgs) Handles Btn_Close.MouseUp
        Btn_Close.Image = My.Resources.XYBrowserRes.Close_E
    End Sub

#End Region

End Class
