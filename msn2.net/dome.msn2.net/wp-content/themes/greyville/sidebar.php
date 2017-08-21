</div>
<div id="right">
<div class="bloccogrigio">
<span class="join"><a title="Home" alt="Home" href="<?php echo get_option('home'); ?>/">home</a><a title="Subscibe to RSS Feed" alt="Subscribe to RSS Feed" href="<?php bloginfo('rss2_url'); ?>">Subscribe to RSS</a></span>
<div class="clear"></div>
</div>
<div class="clear"></div>
<div id="wrapper">
	<div id="content">
	<div class="tab"><h3 class="tabtxt" title="recent posts"><a href="#">Posts</a></div></h3>
	<div class="tab"><h3 class="tabtxt" title="recent Comments"><a href="#">Comments</a></h3></div>
	<div class="tab"><h3 class="tabtxt" title="Browse Categories"><a href="#">Categories</a></h3></div>
	<div class="tab"><h3 class="tabtxt" title="View all Tags"><a href="#">Tags</a></h3></div>
	<div class="boxholder"><div class="box"><p><?php query_posts('showposts=11'); ?><?php while (have_posts()) : the_post(); ?><li><span class="title"><a href="<?php the_permalink() ?>" title="<?php the_title(); ?>"><?php the_title(); ?></a></span><br /><span class="meta"><?php the_time('l, F j, Y'); ?> - <a href="<?php the_permalink() ?>#comments" title="Jump to the comments"><?php comments_number('Post a Comment','1 Comment','% Comments'); ?></a></span></li><?php endwhile; ?></p></div><div class="box"><p><?php
				global $wpdb;
				$sql = "SELECT DISTINCT ID, post_title, post_password, comment_ID,
				comment_post_ID, comment_author, comment_date, comment_approved,
				comment_type,comment_author_url,
				SUBSTRING(comment_content,1,80) AS com_excerpt
				FROM $wpdb->comments
				LEFT OUTER JOIN $wpdb->posts ON ($wpdb->comments.comment_post_ID =
				$wpdb->posts.ID)
				WHERE comment_approved = '1' AND comment_type = '' AND
				post_password = ''
				ORDER BY comment_date DESC
				LIMIT 8";
				$comments = $wpdb->get_results($sql);
				$output = $pre_HTML;
				foreach ($comments as $comment) {
				$output .= "\n<li><span class='title comment_excerpt'><a href=\"" . get_permalink($comment->ID) .
				"#comment-" . $comment->comment_ID . "\" title=\"on " .
				$comment->post_title . "\">" .strip_tags($comment->com_excerpt).
				"...</a></span><br/>" . "<span class='meta'>Said ".strip_tags($comment->comment_author)
				." on ".strip_tags($comment->comment_date)."</span></li>";
				}
				$output .= $post_HTML;
				echo $output;?></p>
		</div><div class="box"><p><ul><span class="title"><?php wp_list_categories(''); ?></span></ul></p></div><div class="box"><p><span class="title"><?php wp_tag_cloud(''); ?></span></p></div></div>
</div>
</div>
<script type="text/javascript">
	Element.cleanWhitespace('content');
	init();
</script>
<div class="searchform"><form id="searchform" method="get" action="<?php echo $_SERVER['PHP_SELF']; ?>"><div><label for="s">Search:</label><input name="s" id="s" class="text" type="text"> <input value="find" class="submit" type="submit"></div></form></div>

<div class="clear"></div>

<div class="blocchisidebar">
<div class="left">
<ul><?php if ( !function_exists('dynamic_sidebar')
        || !dynamic_sidebar() ) : ?>
<?php endif; ?>
</ul>
</div>


<div class="right">
<ul><?php if ( !function_exists('dynamic_sidebar')
        || !dynamic_sidebar('right') ) : ?>
<?php endif; ?>
</ul>

</div>
</div>
<div class="clear"></div>





		<?php get_footer(); ?>