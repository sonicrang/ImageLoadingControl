   M S B A M L   `   `   `  ����N  JImageLoadingControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ImageLoadingControlImageLoadingControl  5      -    'ImageLoadingControl.ImageLoadingControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:ImageLoadingControl     5        � 	Resources  5	       g�Name   waiting �� 
TargetName �� TargetProperty W� From	 W� To g� Duration g� RepeatBehavior��%�   &	        �� 5
   
   $ waiting��6%   .��5      W� $ SpinnerRotate��6   $ (RotateTransform.Angle)�6D   $ 0X�6x   $	 359X�6�   $ 0:0:02D�6�   $ Forever��6�   5      5      .��5      � .+�5   
   � -   5         �Name$ 	gridError��5      
�������.+�5      (� .��5      �� $��暂无图片��6   $	��5,0q�6'   
�������64   5      5      � 5   
   .+�5      �� -   5        $��imageLoading��5      $y�	Collapsed=�6(   $��Image/loading.png��6?   	 J� RenderTransformOrigin$	 0.5,0.5&�6Z   $
��Fill=�6z   $��21��6�   $��21��6�   z�5      �� 5      -   5        SpinnerRotate��
 �� Angle$
 0Q�5   =   5      �� 5      -   5        $��	imageShow��5      $	 0.5,0.5&�6%   5      5      5      