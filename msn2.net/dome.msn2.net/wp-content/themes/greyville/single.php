<?php 
get_header();
?>
<div id="post">	
<?php if (have_posts()) : while (have_posts()) : the_post(); ?>
<div class="clear"></div>
Posted by <?php the_author() ?> on <?php the_time('F jS, Y') ?> :: Filed under <?php the_category(',') ?><?php the_tags('<br />Tags :: '); ?>
<hr size="1" color="#eeeeee" noshade>
</div>
<div class="clear"></div>
<div class="clear"></div>
<div class="right"><?php previous_post_link('%link') ?></div>
<div class="left"><?php next_post_link('%link') ?></div>
</div>
<div class="clear"></div>
<div class="clear"></div>
<?php endif; ?>
</div>
<?php get_sidebar(); ?>