﻿Imports System.Runtime.InteropServices

Module DrawImageModule

    '服务于系统内所有需要绘制Alpha通道绘制图像的窗体
    '被绘制的窗体需要覆写：
    'Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams

#Region "声明区"
    Public Const AC_SRC_OVER As Byte = &H0
    Public Const AC_SRC_ALPHA As Byte = &H1
    Public Const ULW_ALPHA As Int32 = &H2
    Public Const WS_EX_LAYERED As Int32 = &H80000
    Private Const ULW_COLORKEY As Int32 = &H1
    Private Const ULW_OPAQUE As Int32 = &H4

    <StructLayout(LayoutKind.Sequential)>
    Public Structure mySize
        Private cx As Int32
        Private cy As Int32

        Public Sub New(ByVal cx As Int32, ByVal cy As Int32)
            Me.cx = cx
            Me.cy = cy
        End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure myPoint
        Private x As Int32
        Private y As Int32

        Public Sub New(ByVal x As Int32, ByVal y As Int32)
            Me.x = x
            Me.y = y
        End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure BLENDFUNCTION
        Public BlendOp As Byte
        Public BlendFlags As Byte
        Public SourceConstantAlpha As Byte
        Public AlphaFormat As Byte
    End Structure

    Public Declare Auto Function GetDC Lib "user32.dll" (ByVal hWnd As IntPtr) As IntPtr
    Public Declare Auto Function CreateCompatibleDC Lib "gdi32.dll" (ByVal hDC As IntPtr) As IntPtr
    Public Declare Auto Function SelectObject Lib "gdi32.dll" (ByVal hDC As IntPtr, ByVal hObject As IntPtr) As IntPtr
    Public Declare Auto Function UpdateLayeredWindow Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal hdcDst As IntPtr, ByRef pptDst As myPoint, ByRef psize As mySize, ByVal hdcSrc As IntPtr, ByRef pprSrc As myPoint, ByVal crKey As Int32, ByRef pblend As BLENDFUNCTION, ByVal dwFlags As Int32) As Boolean
    Public Declare Auto Function ReleaseDC Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal hDC As IntPtr) As Integer
    Public Declare Auto Function DeleteObject Lib "gdi32.dll" (ByVal hObject As IntPtr) As Boolean
    Public Declare Auto Function DeleteDC Lib "gdi32.dll" (ByVal hdc As IntPtr) As Boolean
#End Region

#Region "执行函数"

    ''' <summary>
    ''' 使用 UpdateLayeredWindow 把带有Alpha通道的图像绘制在窗体
    ''' </summary>
    ''' <param name="FormToDraw">要绘制到的窗体</param>
    ''' <param name="AlphaImage">带Alpha通道的图像</param>
    Public Sub DrawImage(ByVal FormToDraw As Form, ByVal AlphaImage As Bitmap)
        On Error Resume Next
        Dim hDC1 As IntPtr = GetDC(IntPtr.Zero)
        Dim hDC2 As IntPtr = CreateCompatibleDC(hDC1)
        Dim hBitmap1 As IntPtr = IntPtr.Zero
        Dim hBitmap2 As IntPtr = IntPtr.Zero

        hBitmap1 = AlphaImage.GetHbitmap(Color.FromArgb(0))
        hBitmap2 = SelectObject(hDC2, hBitmap1)
        Dim blend As New BLENDFUNCTION()
        With blend
            .BlendOp = AC_SRC_OVER
            .BlendFlags = 0
            .AlphaFormat = AC_SRC_ALPHA
            .SourceConstantAlpha = True
        End With
        Call UpdateLayeredWindow(FormToDraw.Handle, hDC1, New myPoint(FormToDraw.Left, FormToDraw.Top), New mySize(AlphaImage.Width, AlphaImage.Height), hDC2, New myPoint(0, 0), 0, blend, ULW_ALPHA)

        Call ReleaseDC(IntPtr.Zero, hDC1)
        If hBitmap1 <> IntPtr.Zero Then
            Call SelectObject(hDC2, hBitmap2)
            Call DeleteObject(hBitmap1)
        End If
        Call DeleteDC(hDC2)
    End Sub
#End Region

End Module
