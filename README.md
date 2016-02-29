**ImageLoadingControl使用说明**

控件提供`Source`属性，可Binding图片Url或Path

	<ImageLoadingControl:ImageLoadingControl HorizontalAlignment="Left" Height="200" Margin="30,68,0,0" VerticalAlignment="Top" Width="200"  Source="{Binding ImageUrl1}"/>
	<ImageLoadingControl:ImageLoadingControl HorizontalAlignment="Left" Height="200" Margin="303,68,0,0" VerticalAlignment="Top" Width="200" Source="{Binding ImageUrl2}"/>
	<ImageLoadingControl:ImageLoadingControl HorizontalAlignment="Left" Height="200" Margin="556,68,0,0" VerticalAlignment="Top" Width="200" Source="{Binding ImageUrl3}"/>

	private void button_Click(object sender, RoutedEventArgs e)
    {
       ImageUrl1 = @"https://pixabay.com/static/uploads/photo/2016/02/09/13/45/rock-carvings-1189288_960_720.jpg";
       ImageUrl2 = @"https://pixabay.com/static/uploads/photo/2016/02/14/14/32/construction-1199586_960_720.jpg";
       ImageUrl3 = @"c:\test.jpg";
    }


下图分别显示了控件加载中，加载完成，加载失败三种状态：

<img source="https://github.com/sonicrang/ImageLoadingControl/blob/master/imagecontrol.png?raw=true" />

[控件代码和demo](https://github.com/sonicrang/ImageLoadingControl)
