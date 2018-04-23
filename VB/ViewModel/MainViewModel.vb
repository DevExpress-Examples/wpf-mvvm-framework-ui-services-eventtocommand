Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.DataAnnotations
Imports DevExpress.Mvvm.POCO
Imports System.Collections.ObjectModel

Namespace Example.ViewModel
    <POCOViewModel> _
    Public Class MainViewModel
        Public Overridable Property Persons() As ObservableCollection(Of Person)
        Protected Overridable ReadOnly Property MessageBoxService() As IMessageBoxService
            Get
                Return Nothing
            End Get
        End Property
        Public Sub Initialize()
            Persons = New ObservableCollection(Of Person)()
            Persons.Add(ViewModelSource.Create(Function() New Person() With {.FirstName = "John", .LastName = "Smith"}))
            Persons.Add(ViewModelSource.Create(Function() New Person() With {.FirstName = "Alex", .LastName = "Carter"}))
        End Sub
        Public Sub Edit(ByVal person As Person)
            MessageBoxService.Show(String.Format("{0} {1}", person.FirstName, person.LastName))
        End Sub
        Public Function CanEdit(ByVal person As Person) As Boolean
            Return person IsNot Nothing
        End Function
    End Class
    Public Class Person
        Public Property FirstName() As String
        Public Property LastName() As String
    End Class
End Namespace
