Module NativeFunction

    <System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint:="GetDC")> _
    Public Function GetDC(<System.Runtime.InteropServices.InAttribute()> ByVal hWnd As System.IntPtr) As System.IntPtr
    End Function
    <System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint:="CreateCompatibleDC")> _
    Public Function CreateCompatibleDC(<System.Runtime.InteropServices.InAttribute()> ByVal hdc As System.IntPtr) As System.IntPtr
    End Function
    <System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint:="CreateCompatibleBitmap")> _
       Public Function CreateCompatibleBitmap(<System.Runtime.InteropServices.InAttribute()> ByVal hdc As System.IntPtr, ByVal cx As Integer, ByVal cy As Integer) As System.IntPtr
    End Function

    <System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint:="SelectObject")> _
    Public Function SelectObject(<System.Runtime.InteropServices.InAttribute()> ByVal hdc As System.IntPtr, <System.Runtime.InteropServices.InAttribute()> ByVal h As System.IntPtr) As System.IntPtr
    End Function
    <System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint:="DeleteDC")> _
    Public Function DeleteDC(<System.Runtime.InteropServices.InAttribute()> ByVal hdc As System.IntPtr) As <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
    End Function

    <System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint:="DeleteObject")> _
    Public Function DeleteObject(<System.Runtime.InteropServices.InAttribute()> ByVal ho As System.IntPtr) As <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
    End Function
    <System.Runtime.InteropServices.DllImportAttribute("gdi32.dll", EntryPoint:="BitBlt")> _
    Public Function BitBlt(<System.Runtime.InteropServices.InAttribute()> ByVal hdc As System.IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, <System.Runtime.InteropServices.InAttribute()> ByVal hdcSrc As System.IntPtr, ByVal x1 As Integer, ByVal y1 As Integer, ByVal rop As UInteger) As <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
    End Function
    <System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint:="ReleaseDC")> _
    Public Function ReleaseDC(<System.Runtime.InteropServices.InAttribute()> ByVal hWnd As System.IntPtr, <System.Runtime.InteropServices.InAttribute()> ByVal hDC As System.IntPtr) As Integer
    End Function

    Public Const SRCCOPY As Integer = 13369376

End Module
