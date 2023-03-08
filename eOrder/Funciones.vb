Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Xml

Module Funciones

    Public Function EnviaMail(ByVal TrackId As String) As String
        Dim dtResp As DataTable
        Dim MailTo, MailCC, MailResp As String
        MailTo = ""
        MailCC = ""
        MailResp = ""
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE " & ToolReadSettings("sp_eO_Destinatarios", "[DESARROLLOSGVI].[dbo].[sp_eO_Destinatarios]") & " @TrackId")
                cmd.Parameters.AddWithValue("@TrackId", TrackId)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "ORDR"
                        sda.Fill(dt)
                        dtResp = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            Try
                If dtResp.Rows.Count >= 1 Then
                    MailTo = dtResp.Rows.Item(0).Item(0).ToString
                    MailCC = dtResp.Rows.Item(0).Item(1).ToString
                    If MailTo.Contains("@") And MailCC.Contains("@") Then
                        MailResp = SendMail(MailTo, MailCC, TrackId)
                    Else
                        MailResp = ""
                    End If
                    If MailResp = "OK" Then
                        MailResp = "MAIL: Se ha enviado una notificación a: <BR />" & MailTo & MailCC
                    Else
                        If MailResp <> "" Then
                            MailResp = "<b>ERROR:</b> Ha ocurrido un error al enviar el correo electrónico a: <BR />" & MailTo & MailCC & " [" & MailResp & "]"
                        End If
                    End If
                End If
            Catch ex As Exception
                MailResp = "<b>ERROR:</b> Ha ocurrido un error al enviar el correo electrónico a: <BR />" & MailTo & MailCC & " [" & ex.Message & "]"
            End Try
            ClearMemory()
        End Using
        Return MailResp
    End Function

    Private Function SendMail(ByVal Destinatarios As String, ByVal CCDestinatarios As String, ByVal NumAtCard As String) As String
        Dim Email As New System.Net.Mail.MailMessage
        Dim MailServer As New System.Net.Mail.SmtpClient("smtp.office365.com")
        Dim SenderAddress As New System.Net.Mail.MailAddress(ToolReadSettings("SMTPUserName", "facturaelectronica@gvi.com.mx"), ToolReadSettings("SMTPTitle", "Grupo Venta Internacional"))
        Dim SendResponse As String = String.Empty
        Try
            Dim Body As String = "<html xmlns=""http://www.w3.org/1999/xhtml""><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /><title>Grupo Venta Internacional</title><style type=""text/css"">body {font-family: Verdana, Geneva, sans-serif;font-size: 12px; background-color:#F5F5F5;} td,th {font-family: Verdana, Geneva, sans-serif;font-size: 12px; background-color:#FFF;} table {font-family: Verdana, Geneva, sans-serif;font-size: 12px; background-color:#FFF; width:790px;}</style></head><body><font face=""verdana""><table border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0""><tr><td align=""center""><table border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0""><tr><td colspan=""2"" align=""center""><font color=""#ee7103""><h3>SERVICIO ELECTRONICO DE ATENCION A CLIENTES</h3></font></td></tr><tr><td colspan=""2"" align=""center"">&nbsp;</td></tr><tr bgcolor=""#FFFFFF""><td colspan=""2"" align=""right""><font color=""#6e6e6e"">FECHAHORA&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></td></tr><tr><td colspan=""2"" align=""center"">&nbsp;</td></tr><tr><td align=""center""><font color=""#6e6e6e""><p>Se ha realizado el siguiente <b>Pedido #</b></p><h1>TRACKID</h1></font></td><td align=""center""><img src=""http://it.gvi.com.mx/images/caro.png"" style=""width:175px;""></td></tr><tr><td colspan=""2"" align=""center"">&nbsp;</td></tr><tr><td colspan=""2"" align=""center""><font color=""#6e6e6e""><p>Conozca en cada momento el estado que guarda cada uno de sus pedidos, utilizando la opción:</p></font></td></tr><tr><td align=""center""><font color=""#6e6e6e""><p><a href=""http://it.gvi.com.mx/eOrders/Tracking?eOrder=TRACKID""><img src=""http://it.gvi.com.mx/images/checo.png"" style=""width:128px;"" /></a><br /><b>UBICAR</b></p></font></td><td align=""center""><font color=""#6e6e6e""><p><a href=""http://it.gvi.com.mx/eOrders/Tracking?eOrder=TRACKID""><img src=""https://chart.googleapis.com/chart?cht=qr&chl=http://it.gvi.com.mx/eOrders/Tracking?eOrder=TRACKID&chs=122x122&choe=UTF-8&chld=H|0"" style=""width:122px;"" /></a><br /><b>Escanee</b> para conocer el estado de su pedido</p></font></td></tr></table><hr color=""#ee7103"" size=""8px""><img src=""http://it.gvi.com.mx/images/footmail.png"" style=""width:790px;"" /></td></tr></table></font></body></html>"
            'ToolReadSettings("BodyMail", "&lt;html&gt;&lt;head&gt;&lt;title&gt;Grupo Venta Internacional&lt;&#47;title&gt;&lt;&#47;head&gt;&lt;body style='font-family: Arial,sans-serif;' &gt;&lt;font face=&#39;verdana&#39;&gt;&lt;table width=&#39;100%&#39; border=&#39;0&#39; align=&#39;center&#39; cellpadding=&#39;0&#39; cellspacing=&#39;0&#39;&gt;&lt;tr&gt;&lt;td align=&#39;center&#39; valign=&#39;middle&#39;&gt;&lt;table width=&#39;100%&#39; border=&#39;0&#39; align=&#39;center&#39; cellpadding=&#39;0&#39; cellspacing=&#39;0&#39;&gt;&lt;tr&gt;&lt;td align=&#39;center&#39; valign=&#39;middle&#39;&gt; &lt;&#47;td&gt;&lt;td align=&#39;right&#39; valign=&#39;middle&#39;&gt;&lt;font color=&#39;#6e6e6e&#39; size=&#39;3&#39;&gt;FECHAHORA      &lt;&#47;font&gt;&lt;&#47;td&gt;&lt;&#47;tr&gt;&lt;tr&gt;&lt;td colspan=&#39;2&#39; align=&#39;center&#39; valign=&#39;middle&#39;&gt; &lt;&#47;td&gt;&lt;&#47;tr&gt;&lt;tr&gt;&lt;td colspan=&#39;2&#39; align=&#39;center&#39; valign=&#39;middle&#39;&gt;&lt;font color=&#39;#ee7103&#39;&gt;&lt;h3&gt;SERVICIO ELECTRONICO DE ATENCION A CLIENTES&lt;&#47;h3&gt;&lt;&#47;font&gt;&lt;&#47;td&gt;&lt;&#47;tr&gt;&lt;tr&gt;&lt;td colspan=&#39;2&#39; align=&#39;center&#39; valign=&#39;middle&#39;&gt; &lt;&#47;td&gt;&lt;&#47;tr&gt;&lt;tr&gt;&lt;td align=&#39;center&#39; valign=&#39;middle&#39;&gt;&lt;font color=&#39;#6e6e6e&#39; size=&#39;3&#39;&gt;&lt;p&gt;Se ha realizado el siguiente &lt;b&gt;Pedido #&lt;&#47;b&gt;&lt;&#47;p&gt;&lt;h1&gt;TRACKID&lt;&#47;h1&gt;&lt;&#47;font&gt;&lt;&#47;td&gt;&lt;td align=&#39;center&#39; valign=&#39;middle&#39;&gt;&lt;img src=&#39;http:&#47;&#47;it.gvi.com.mx&#47;images&#47;caro.png&#39; width=&#39;300px&#39;&gt;&lt;&#47;td&gt;&lt;&#47;tr&gt;&lt;tr&gt;&lt;td colspan=&#39;2&#39; align=&#39;center&#39; valign=&#39;middle&#39;&gt; &lt;&#47;td&gt;&lt;&#47;tr&gt;&lt;tr&gt;&lt;td colspan=&#39;2&#39; align=&#39;center&#39; valign=&#39;middle&#39;&gt;&lt;font color=&#39;#6e6e6e&#39; size=&#39;2&#39;&gt;&lt;p&gt;Conozca en cada momento el estado que guarda cada uno de sus pedidos, utilizando la opción:&lt;br &#47;&gt; &lt;a href=&#39;http:&#47;&#47;it.gvi.com.mx&#47;eOrders&#47;Tracking?eOrder=TRACKID&#39;&gt;&lt;img src=&#39;http:&#47;&#47;it.gvi.com.mx&#47;images&#47;checo.png&#39; width=&#39;128px&#39; &#47;&gt;&lt;&#47;a&gt;&lt;br &#47;&gt;&lt;b&gt;UBICAR&lt;&#47;b&gt;&lt;&#47;p&gt;&lt;&#47;font&gt;&lt;&#47;td&gt;&lt;&#47;tr&gt;&lt;&#47;table&gt;&lt;hr color=&#39;#ee7103&#39; size=&#39;8px&#39;&gt;&lt;img src=&#39;http:&#47;&#47;it.gvi.com.mx&#47;images&#47;footmail.png&#39; width=&#39;100%&#39; &#47;&gt;&lt;&#47;td&gt;&lt;&#47;tr&gt;&lt;&#47;table&gt;&lt;&#47;font&gt;&lt;&#47;body&gt;&lt;&#47;html&gt;")
            Body = Body.Replace("TRACKID", NumAtCard)
            Dim fecha As String = Now.ToString("F", System.Globalization.CultureInfo.CreateSpecificCulture("es-MX"))
            Body = Body.Replace("FECHAHORA", fecha)
            With Email
                .From = SenderAddress
                .Subject = "Pedido # " & NumAtCard
                .Body = Body
                .IsBodyHtml = True
            End With
            'Destinatario(s) separados por ;
            Dim Correos() As String = Destinatarios.Split(";")
            For Each Correo In Correos
                If Correo <> "" Then
                    Email.To.Add(New System.Net.Mail.MailAddress(Correo, Correo))
                End If
            Next
            Dim CCCorreos() As String = CCDestinatarios.Split(";")
            For Each CCCorreo In CCCorreos
                If CCCorreo <> "" Then
                    Email.CC.Add(New System.Net.Mail.MailAddress(CCCorreo, CCCorreo))
                End If
            Next
            MailServer.EnableSsl = True
            MailServer.Port = 587
            MailServer.Credentials = New System.Net.NetworkCredential(ToolReadSettings("SMTPUserName", "facturaelectronica@gvi.com.mx"), ToolReadSettings("SMTPPwd", "Fola2472"))
            MailServer.Send(Email)
            SendResponse = "OK"
        Catch ex As Exception
            SendResponse = ex.Message
        Finally
            If Not Email Is Nothing Then
                Email.Dispose()
            End If
            MailServer = Nothing
        End Try
        ClearMemory()
        Return SendResponse
    End Function

    Public Function ObtieneItems(ByVal CodCli As String, ByVal CodItem As String, ByVal Alm As String, ByVal SerLot As String, ByVal SlpCode As String, ByVal TP As String) As DataTable
        Dim Resp As DataTable
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE " & ToolReadSettings("sp_eO_Items_v2", "[DESARROLLOSGVI].[dbo].[sp_eO_Items_v2]") & " @CardCode, @ItemCode, @WhsCode, @LoteSerie, @SlpCode, @TPersonaNum")
                cmd.Parameters.AddWithValue("@CardCode", CodCli)
                cmd.Parameters.AddWithValue("@ItemCode", CodItem)
                cmd.Parameters.AddWithValue("@WhsCode", Alm)
                cmd.Parameters.AddWithValue("@LoteSerie", SerLot)
                cmd.Parameters.AddWithValue("@SlpCode", SlpCode)
                cmd.Parameters.AddWithValue("@TPersonaNum", TP)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "OITM"
                        sda.Fill(dt)
                        Resp = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            ClearMemory()
        End Using
        Return Resp
    End Function

    Public Function ObtieneDirs(ByVal CodCli As String) As DataTable
        Dim Resp As DataTable
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE " & ToolReadSettings("sp_eO_Addrs", "[DESARROLLOSGVI].[dbo].[sp_eO_Addrs]") & " @CardCode")
                cmd.Parameters.AddWithValue("@CardCode", CodCli)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "CRD1"
                        sda.Fill(dt)
                        Resp = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            ClearMemory()
        End Using
        Return Resp
    End Function

    Public Sub ObtieneLista(ByVal CodCliente As String, ByRef listTP As ListBox)
        Dim dtResp As DataTable
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE " & ToolReadSettings("sp_eO_Listas", "[DESARROLLOSGVI].[dbo].[sp_eO_Listas]") & " @CardCode")
                cmd.Parameters.AddWithValue("@CardCode", CodCliente)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "OCRD"
                        sda.Fill(dt)
                        dtResp = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            Try
                If dtResp.Rows.Count >= 1 Then
                    listTP.Items.Clear()
                    listTP.DataSource = dtResp
                    listTP.DataTextField = "Nombre"
                    listTP.DataValueField = "Codigo"
                    listTP.DataBind()
                Else
                    'SQL Caido
                End If
            Catch ex As Exception
                ' = ex.Message
            End Try
            ClearMemory()
        End Using
    End Sub

    Public Function ObtieneCardName(ByVal CodCliente As String, ByVal EsRefBank As Boolean) As String
        Dim getData As String = ""
        Dim dtResp As DataTable
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE " & ToolReadSettings("sp_eO_CardName", "[DESARROLLOSGVI].[dbo].[sp_eO_CardName]") & " @CardCode")
                cmd.Parameters.AddWithValue("@CardCode", CodCliente)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "OCRD"
                        sda.Fill(dt)
                        dtResp = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            Try
                If dtResp.Rows.Count >= 1 Then
                    If EsRefBank Then
                        getData = dtResp.Rows.Item(0).Item(1).ToString
                    Else
                        getData = dtResp.Rows.Item(0).Item(0).ToString
                    End If
                Else
                    'SQL Caido
                    getData = "El datatable no contiene información"
                End If
            Catch ex As Exception
                getData = ex.Message
            End Try
            ClearMemory()
        End Using
        Return getData
    End Function
    Public Function ObtieneWhsName(ByVal CodAlmacen As String) As String
        Dim getWhsName As String = ""
        Dim dtResp As DataTable
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE " & ToolReadSettings("sp_eO_WhsName", "[DESARROLLOSGVI].[dbo].[sp_eO_WhsName]") & " @WhsCode")
                cmd.Parameters.AddWithValue("@WhsCode", CodAlmacen)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "OWHS"
                        sda.Fill(dt)
                        dtResp = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            Try
                If dtResp.Rows.Count >= 1 Then
                    getWhsName = dtResp.Rows.Item(0).Item(0).ToString
                Else
                    'SQL Caido
                    getWhsName = "El datatable no contiene información"
                End If
            Catch ex As Exception
                getWhsName = ex.Message
            End Try
            ClearMemory()
        End Using
        Return getWhsName
    End Function

    Public Function IniciarSesion(ByVal Usuario As String, ByVal Pass As String) As DataTable
        Dim Resp As DataTable
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE " & ToolReadSettings("sp_eO_Login", "[DESARROLLOSGVI].[dbo].[sp_eO_Login]") & " @Usuario, @Pass")
                cmd.Parameters.AddWithValue("@Usuario", Usuario)
                cmd.Parameters.AddWithValue("@Pass", Pass)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "OUSR"
                        sda.Fill(dt)
                        Resp = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            ClearMemory()
        End Using
        Return Resp
    End Function

    Public Function ValidaUsr(ByVal Usuario As String) As DataTable
        Dim Resp As DataTable
        Dim constr As String = ConfigurationManager.ConnectionStrings("SQLConnection").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("EXECUTE " & ToolReadSettings("sp_eO_ValidaUsr", "[DESARROLLOSGVI].[dbo].[sp_eO_ValidaUsr]") & " @Usuario")
                cmd.Parameters.AddWithValue("@Usuario", Usuario)
                Using sda As New SqlDataAdapter()
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        dt.TableName = "OUSR"
                        sda.Fill(dt)
                        Resp = dt.Copy()
                        dt.Clear()
                        dt.Dispose()
                    End Using
                    sda.Dispose()
                End Using
                cmd.Dispose()
            End Using
            con.Close()
            con.Dispose()
            ClearMemory()
        End Using
        Return Resp
    End Function

    '//*************************************************************
    '// Limpiamos memoria a nivel proceso
    '//*************************************************************
    Public Declare Auto Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal procHandle As IntPtr, ByVal min As Int32, ByVal max As Int32) As Boolean
    Public Sub ClearMemory()
        Try
            Dim Mem As System.Diagnostics.Process
            Mem = System.Diagnostics.Process.GetCurrentProcess()
            SetProcessWorkingSetSize(Mem.Handle, -1, -1)
        Catch ex As Exception
            'Control de errores
        End Try
    End Sub

    '//*************************************************************
    '// Lee la configuración del sistema
    '//*************************************************************
    Public Function ToolReadSettings(Keys As String, DefaultValue As String) As String
        Dim val As String = ""
        Try
            Dim appSettings = System.Configuration.ConfigurationManager.AppSettings
            For Each key As String In appSettings.AllKeys
                If Keys = key Then
                    val = appSettings(key)
                End If
            Next
            If val = "" Then
                Try
                    val = DefaultValue
                    Dim configFile = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None)
                    Dim settings = configFile.AppSettings.Settings
                    If IsNothing(settings(Keys)) Then
                        settings.Add(Keys, DefaultValue)
                    Else
                        settings(Keys).Value = DefaultValue
                    End If
                    configFile.Save(System.Configuration.ConfigurationSaveMode.Modified)
                    System.Configuration.ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
                Catch e As System.Configuration.ConfigurationErrorsException

                End Try
            End If
        Catch e As System.Configuration.ConfigurationErrorsException

        End Try
        Return val
    End Function

End Module
