<?php // Do not delete these lines
	if ('comments.php' == basename($_SERVER['SCRIPT_FILENAME']))
		die ('Please do not load this page directly. Thanks!');

        if (!empty($post->post_password)) { // if there's a password
            if ($_COOKIE['wp-postpass_' . COOKIEHASH] != $post->post_password) {  // and it doesn't match the cookie
				?>
				
				<p class="nocomments">This post is password protected. Enter the password to view comments.<p>
				
				<?php
				return;
            }
        }

		/* This variable is for alternating comment background */
		$oddcomment = 'alt';
?>

<!-- You can start editing here. -->
<div id="commenti">
					
						<hr class="hidden">
<?php if ($comments) : ?>
	<h3 id="comments"><?php comments_number('No Responses', 'One Response', '% Responses' );?> to &#8220;<?php the_title(); ?>&#8221;</h3> 

<div class="commentosingolo">

	<ol class="commentlist">

	<?php foreach ($comments as $comment) : ?>

		<li class="<?php echo $oddcomment; ?>" id="comment-<?php comment_ID() ?>">
<div class="commentname">
        <?php if(function_exists('get_avatar')) { echo get_avatar($comment, '32'); } ?><?php comment_author_link()?><br /><?php comment_date('F jS, Y') ?> 
		<?php edit_comment_link(__("Edit This"), ''); ?> 
		 
      </div>

					<div class='commenttext'>   

			<div class="commentp"><?php comment_text();?></div>
		</div>
					
    </li>

	<?php /* Changes every other comment to a different class */	
		if ('alt' == $oddcomment) $oddcomment = '';
		else $oddcomment = 'alt';
	?>

	<?php endforeach; /* end for each comment */ ?>

	</ol>
</div>
 <?php else : // this is displayed if there are no comments so far ?>

  <?php if ('open' == $post->comment_status) : ?> 
		<!-- If comments are open, but there are no comments. -->
		
	 <?php else : // comments are closed ?>
		<!-- If comments are closed. -->
		<p class="nocomments">Comments are closed.</p>
		
	<?php endif; ?>
<?php endif; ?>


<?php if ('open' == $post->comment_status) : ?>
<div id="commenti">
<hr class="hidden">

<h3 id="respond">Leave a Reply</h3>
<div class="commentosingolo">
<?php if ( get_option('comment_registration') && !$user_ID ) : ?>
<p>You must be <a href="<?php echo get_option('siteurl'); ?>/wp-login.php?redirect_to=<?php the_permalink(); ?>">logged in</a> to post a comment.</p>


<?php else : ?>

<form action="<?php echo get_option('siteurl'); ?>/wp-comments-post.php" method="post" id="commentform">

<?php if ( $user_ID ) : ?>

<p>Logged in as <a href="<?php echo get_option('siteurl'); ?>/wp-admin/profile.php"><?php echo $user_identity; ?></a>. <a href="<?php echo get_option('siteurl'); ?>/wp-login.php?action=logout" title="Log out of this account">Logout &raquo;</a></p>

<?php else : ?>

<p><input type="text" name="author" class="text" id="author" value="<?php echo $comment_author; ?>" size="15" tabindex="1" />
<label for="author"><small>Your Name <?php if ($req) _e('(*)'); ?></small></label></p>

<p><input type="text" name="email" class="text" id="email" value="<?php echo $comment_author_email; ?>" size="15" tabindex="2" />
<label for="email"><small>Your E-mail <small>(won't be published here)</small> <?php if ($req) _e('(*)'); ?></small></label></p>

<p><input type="text" name="url" id="url" class="text" value="<?php echo $comment_author_url; ?>" size="15" tabindex="3" />
<label for="url"><small>Your Website</small></label></p>

<?php endif; ?>

<p>Type your comment in the box below:</p>

<p><textarea name="comment" id="comment" class="textarea" cols="50" rows="10" tabindex="4"></textarea></p>



<p><input name="submit" type="submit" class="submit" id="submit" tabindex="5" value="Submit" />
<input type="hidden" name="comment_post_ID" value="<?php echo $id; ?>" />
</p>
<?php do_action('comment_form', $post->ID); ?>

</form>
</div>


<?php endif; // If registration required and not logged in ?>
</div>
<?php endif; // if you delete this the sky will fall on your head ?>
</div>