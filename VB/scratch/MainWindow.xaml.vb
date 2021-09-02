Imports System.Windows
Imports System.Windows.Input
Imports DevExpress.Xpf.Charts

Namespace scratch
	Partial Public Class MainWindow
		Inherits Window

		Private Property clickPosition() As Point
		Private startDragging As Point
		Public Property isDragging() As Boolean

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub chart_MouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			startDragging = e.GetPosition(chartControl1)
			Dim hitInfo As ChartHitInfo = chartControl1.CalcHitInfo(startDragging)
			isDragging = hitInfo IsNot Nothing AndAlso hitInfo.InLegend
			DirectCast(sender, UIElement).CaptureMouse()
		End Sub

		Private Sub chart_MouseLeftButtonUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			DirectCast(sender, UIElement).ReleaseMouseCapture()
			isDragging = False
		End Sub

		Private Sub chart_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			If e.LeftButton = MouseButtonState.Pressed AndAlso isDragging Then
				Dim endDragging As Point = e.GetPosition(chartControl1)
				Dim inLegendPosition As Point = e.GetPosition(legend)

				If inLegendPosition.X < 0 Then
					inLegendPosition.X = 0
				ElseIf inLegendPosition.X > legend.ActualWidth Then
					inLegendPosition.X = legend.ActualWidth
				End If

				If inLegendPosition.Y < 0 Then
					inLegendPosition.Y = 0
				ElseIf inLegendPosition.Y > legend.ActualHeight Then
					inLegendPosition.Y = legend.ActualHeight
				End If

				If endDragging.X - inLegendPosition.X > 0 AndAlso endDragging.X + legend.ActualWidth - inLegendPosition.X < chartControl1.ActualWidth Then
						legendTransform.X += endDragging.X - startDragging.X
				End If

				If endDragging.Y - inLegendPosition.Y > 0 AndAlso endDragging.Y + legend.ActualHeight - inLegendPosition.Y < chartControl1.ActualHeight Then
						legendTransform.Y += endDragging.Y - startDragging.Y
				End If

				startDragging = endDragging
			End If
		End Sub

	End Class
End Namespace