<?php
if ( function_exists('register_sidebar') )
register_sidebar(array('name'=>'left',
'before_widget' => '<li>',
'after_widget' => '</li>',
'before_title' => '<div><h3>',
'after_title' => '</h3></div>',
));
register_sidebar(array('name'=>'right',
'before_widget' => '<li>',
'after_widget' => '</li>',
'before_title' => '<div><h3>',
'after_title' => '</h3></div>',
));
?>
