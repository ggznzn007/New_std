   M S B A M L   `   `   `  ����E  A28_WPF_BMI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:_28_WPF_BMI_28_WPF_BMI  5   	       _28_WPF_BMI.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:_28_WPF_BMI     5   	     � Title$(  "BMI(비만도) 측정 프로그램��5   	   $	��500��6"   $	��500��6/   .��5	      � 
��� � � � ColumnDefinitions 5
   
   �� 5      $	��13* �6   �� 5      $��6* �6   .+�5   
   �� $��Center=�6   $��Center=�62   $��95,78,101.6,14.4q�6M   $	��297��6g   $	��378��6s   $��2��6�   .+�5      �� $��10q�6    �� Orientation$ 
Horizontal=�6%   .+�5      �� -   5        $
��lblH��5      $	��100��6%   $��15(�61   .��6?   	키(cm)6F   �� 5      -   5        $��	txtHeight��5      $��15(�6+   $	��150��69   6k   5      �� 5      $��10q�6   $ 
Horizontal=�6%   .+�5      �� -   5        $
��lblW��5      $	��100��6%   $��15(�61   .��6?   
체중(kg)6G   �� 5      -   5        $��	txtWeight��5      $��15(�6+   $	��150��69   6�   5      �� 5      -   5         ��Name$ btnBMI��5      $��171,20,36.8,20q�6%   $��50��6=   $��
BMI 측정��5   *   �� 5      -   5        $ btnClr��5      $��174,5,38.8,5q�6%   $��30��6;   $��Clear��5   *   �� 5      -   5        $��	lblResult��5      $��10,10,2.8,10q�6%   $	��284��6;   $
��Bold"�6G   $��15(�6Y   $	��133��6g   .��6t   
결과는:6z   5      5      5       