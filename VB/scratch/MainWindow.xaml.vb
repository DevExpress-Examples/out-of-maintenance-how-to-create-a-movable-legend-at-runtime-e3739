Imports System.Windows
Imports System.Windows.Input
Imports DevExpress.Xpf.Charts

Namespace scratch

    Public Partial Class MainWindow
        Inherits Window

        Private Property clickPosition As Point

        Private startDragging As Point

        Public Property isDragging As Boolean

        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Private Sub chart_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            startDragging = e.GetPosition(Me.chartControl1)
            Dim hitInfo As ChartHitInfo = Me.chartControl1.CalcHitInfo(startDragging)
            isDragging = hitInfo IsNot Nothing AndAlso hitInfo.InLegend
            CType(sender, UIElement).CaptureMouse()
        End Sub

        Private Sub chart_MouseLeftButtonUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
            CType(sender, UIElement).ReleaseMouseCapture()
            isDragging = False
        End Sub

        Private Sub chart_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
            If e.LeftButton = MouseButtonState.Pressed AndAlso isDragging Then
                Dim endDragging As Point = e.GetPosition(Me.chartControl1)
                Dim inLegendPosition As Point = e.GetPosition(Me.legend)
                If inLegendPosition.X < 0 Then
                    inLegendPosition.X = 0
                ElseIf inLegendPosition.X > Me.legend.ActualWidth Then
                    inLegendPosition.X = Me.legend.ActualWidth
                End If

                If inLegendPosition.Y < 0 Then
                    inLegendPosition.Y = 0
                ElseIf inLegendPosition.Y > Me.legend.ActualHeight Then
                    inLegendPosition.Y = Me.legend.ActualHeight
                End If

                If endDragging.X - inLegendPosition.X > 0 AndAlso endDragging.X + Me.legend.ActualWidth - inLegendPosition.X < Me.chartControl1.ActualWidth Then Me.legendTransform.X += endDragging.X - startDragging.X
                If endDragging.Y - inLegendPosition.Y > 0 AndAlso endDragging.Y + Me.legend.ActualHeight - inLegendPosition.Y < Me.chartControl1.ActualHeight Then Me.legendTransform.Y += endDragging.Y - startDragging.Y
                startDragging = endDragging
            End If
        End Sub
    End Class
End Namespace
