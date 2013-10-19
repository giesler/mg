<?php 
get_header();
?><div id="contenuto"><div id="left">
<div id="post">	
<?php if (have_posts()) : while (have_posts()) : the_post(); ?><div class="post" id="post-<?php the_ID(); ?>"><h1><a href="<?php the_permalink() ?>" rel="bookmark"><?php the_title(); ?></a></h1>
<div class="commenti_top"><?php comments_popup_link('0', '1', '%'); ?></div><div class="clear"></div><div class="content"><?php the_content('<div class="readmore">Read All &raquo;</div>'); ?><div class="clear"></div><hr size="1" color="#eeeeee" noshade>
Posted by <?php the_author() ?> on <?php the_time('F jS, Y') ?> :: Filed under <?php the_category(',') ?><?php the_tags('<br />Tags :: '); ?> <!-- by <?php the_author() ?> --><?php edit_post_link('<br /><b>Edit This</b>'); ?>
<hr size="1" color="#eeeeee" noshade>
<div class="clear"></div></div><?php comments_template(); ?></div><?php endwhile; else: ?><div class="post"><h1><a href="#">Not Found</a></h1><div class="clear"></div><div class="content">The document you were searching for was not found at AdverBox. Try performing another search using the form below:<br /><br /><div class="searchform"><form id="searchform" method="get" action="<?php echo $_SERVER['PHP_SELF']; ?>"><div><input name="s" type="text" class="text" id="s" size="45"> <input value="find" class="submit" type="submit"></div></form></div><div class="clear"></div></div></div><?php endif; ?>
<div id="navigatore">
<div class="right"><?php next_posts_link('Previous Articles') ?></div>
<div class="left"><?php previous_posts_link('Next Articles') ?></div>
</div>
</div><?php get_sidebar(); ?>