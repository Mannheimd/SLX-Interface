   M S B A M L   `   `   `  ����H  DSLX Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:SLX_InterfaceSLX_Interface  5   	       SLX_Interface.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:SLX_Interface     5   	     � Title$  
SLX Search��5   	   $
��auto��6   $
��auto��6*    J� CommandBindings 5	      .��5      �  � 	Resources 5   
      SLX_Interface.CachedData   
cachedData��%   &	          5      6   5      .+�5   
   ]� .+�5      o� $	��Top=�6   .@�5      m� $��_File��6   .@�5      �� m� 5      $��_Exit��6   5      m� 5      $��_Edit��6   .@�5      m�  m� Command$	 Cut��6   m� 5      $
 Copy��6   m� 5      $ Paste��6   m� 5      -   5        $��Edit field visibility��5      6>   5      m� 5      $��_View��6   5      5      5      �� 5   
   -   5         ��Name$ 
tabControl��5      $��2��6)   $��0,20,0,0q�6:   .@�5       �� -   5        $��accountsTab��5    (   $��Accounts��6   .��5!      � .+�5"      �� -   5         ��Name$ accountSearchFields��5"      $
��Left=�6<   $��5,5,0,0q�6W   $	��Top=�6h   $	��120��6�    �� SelectedIndex$ 0��6�   �� 5#      -   5        $ accountSearchOperator��5#      $
��Left=�6>   $��	130,5,0,0q�6Y   $	��Top=�6l   $	��120��6�   $ 0��6�   .@�5$      �� $��Contains��6'   �� 5%      $��Equals��6'   �� 5&      $��Starts With��6'   5'      �� 5(      -   5         ��Name$ accountSearchBox��5(      $
��Left=�68   $��22��6S    �� TextWrapping$
 Wrap=�6`   $	��Top=�6t   $	��120��6�   $��	255,5,0,0q�6�   $��13.333(�6�   �� 5)      -   5        	 ��Name$	 accountSearchButton��5)      $��Search��6:   $
��Left=�6K   $��22��6f   $	��Top=�6r   $��52��6�   $��	380,5,0,0q�6�   6�   &   System.Windows.Controls.DataGrid  5*      -   5        
  Name$
 accountSearchResultsList��5*      $��	5,32,5,30q�6A    � DataContext$ staticResource cachedData��6T   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6�      SelectionMode$ Single=�6�      
IsReadOnly .  �� AlternationCount$ 1��6-     AlternatingRowBackground �	#FFDCDCFF   HeadersVisibility$ Column=�6g     GridLinesVisibility$ Vertical=�6�  ��6|   ��  �� Path$ accounts�6�  �� 5+      -	   5         ��Name$ accountSearchResultsCount��5+      $��	No Search��6?   $
��Left=�6S   $��5,0,0,5q�6n   $��Bottom=�6   $��25��6�   $	��120��6�   � 5,      -
   5         �Name$ accountSearchProgress��5,   "   $��Right=�6A   $��20��6]   $��0,0,5,5q�6i   $��Bottom=�6z   $	��120��6�   $y�Hidden=�6�    � IsIndeterminate . 6�   5-      5.      �� 5/      -   5        $��
ticketsTab��5/   '   $��Tickets��6   .��50      � .+�51      �� -   5        $ ticketSearchSubData��51      $
��Left=�6<   $��5,5,0,0q�6W   $	��Top=�6h   $	��120��6�   $��20��6�   �� 52      -   5        $ ticketSearchOperator��52      $
��Left=�6=   $��	130,5,0,0q�6X   $	��Top=�6k   $	��120��6�   $��20��6�   $ 0��6�   .@�53      �� $��Contains��6'   �� 54      $��Equals��6'   �� 55      $��Starts With��6'   56      �� 57      -   5        $ ticketSearchBox��57      $
��Left=�67   $��20��6R   $
 Wrap=�6^   $	��Top=�6r   $	��120��6�   $��	255,5,0,0q�6�   6�   �� 58      -   5        $	 ticketSearchButton��58      $��Search��69   $
��Left=�6J   $��20��6e   $	��Top=�6q   $��52��6�   $��	380,5,0,0q�6�   6�   �� 59      -   5         ��Name$! ticketSearchResultsListView��59      $��	5,30,5,30q�6D    �� View 5:      �� 5;      .P�5<   "   �� $��SLX Account ID��61    �� DisplayMemberBinding 6I   �� *
SDataKey+�� 5=   "   $��Account Name��61    6G   �� *AccountName+�� 5>   "   $��Account Number��61    6I   �� *
CustomerId+5?      5A      �� 5B      -   5        $ ticketSearchResultsCount��5B      $��	No search��6>   $
��Left=�6R   $��5,0,0,5q�6m   $��Bottom=�6~   $��25��6�   $	��120��6�   � 5C      -   5        $ ticketSearchProgress��5C   "   $��Right=�6@   $��20��6\   $��0,0,5,5q�6h   $��Bottom=�6y   $	��120��6�   $y�Hidden=�6�    . 6�   5D      5E      5F      5G      5H      