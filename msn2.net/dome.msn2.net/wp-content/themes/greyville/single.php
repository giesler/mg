<?php 
get_header();
?><div id="contenuto"><div id="left">
<div id="post">	
<?php if (have_posts()) : while (have_posts()) : the_post(); ?><div class="post" id="post-<?php the_ID(); ?>"><h1><a href="<?php the_permalink() ?>" rel="bookmark"><?php the_title(); ?></a></h1>
<div class="clear"></div><div class="content"><?php the_content('Read the rest of this entry &raquo;'); ?><div class="clear"></div><hr size="1" color="#eeeeee" noshade>
Posted by <?php the_author() ?> on <?php the_time('F jS, Y') ?> :: Filed under <?php the_category(',') ?><?php the_tags('<br />Tags :: '); ?><br /><?php if (('open' == $post-> comment_status) && ('open' == $post->ping_status)) {							// Both Comments and Pings are open ?>							You can <a href="#respond">leave a response</a>, or <a href="<?php trackback_url(); ?>" rel="trackback">trackback</a> from your own site.						<?php } elseif (!('open' == $post-> comment_status) && ('open' == $post->ping_status)) {							// Only Pings are Open ?>							Responses are currently closed, but you can <a href="<?php trackback_url(); ?> " rel="trackback">trackback</a> from your own site.						<?php } elseif (('open' == $post-> comment_status) && !('open' == $post->ping_status)) {							// Comments are open, Pings are not ?>							You can skip to the end and leave a response. Pinging is currently not allowed.						<?php } elseif (!('open' == $post-> comment_status) && !('open' == $post->ping_status)) {							// Neither Comments, nor Pings are open ?>							Both comments and pings are currently closed.						<?php } edit_post_link('<br />Edit this entry.','',''); ?>
<hr size="1" color="#eeeeee" noshade>
</div>
<div class="clear"></div>
<div class="clear"></div><div id="navigatore">
<div class="right"><?php previous_post_link('%link') ?></div>
<div class="left"><?php next_post_link('%link') ?></div>
</div>
<div class="clear"></div>
<div class="clear"></div><?php comments_template(); ?></div><?php endwhile; else: ?><div class="post"><h2>Not Found</h2><h3>Sorry :-(</h3><h4>Your search did not trigger any results. Try to perform another search.</h4></div>
<?php endif; ?>
</div>
<?php get_sidebar(); ?>