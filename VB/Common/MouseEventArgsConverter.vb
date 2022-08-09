Imports DevExpress.Mvvm.UI
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Linq

Namespace Example.Common

    Public Class ListBoxEventArgsConverter
        Inherits EventArgsConverterBase(Of MouseEventArgs)

        Protected Overrides Function Convert(ByVal sender As Object, ByVal args As MouseEventArgs) As Object
            Dim element = LayoutTreeHelper.GetVisualParents(CType(args.OriginalSource, DependencyObject), CType(sender, DependencyObject)).OfType(Of ListBoxItem)().FirstOrDefault()
            Return If(element IsNot Nothing, element.DataContext, Nothing)
        End Function
    End Class
End Namespace
