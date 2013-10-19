<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
<?php if(is_home('') ) { ?><title><?php bloginfo('name'); ?> - <?php bloginfo('description'); ?></title><?php } else { ?><title><?php the_title(); ?> - <?php bloginfo('name'); ?></title><?php } ?><?php if(is_home('') ) { ?><meta name="description" content="<?php bloginfo('name'); ?> - <?php bloginfo('description'); ?>" /><?php } else { ?><meta name="description" content="<?php the_title(); ?> - <?php bloginfo('name'); ?> <?php bloginfo('description'); ?>." /><?php } ?><?php if(is_home('') ) { ?><meta name="keywords" content="<?php bloginfo('name'); ?> - <?php bloginfo('description'); ?>" /><?php } else { ?><meta name="keywords" content="<?php the_title(); ?>,<?php foreach((get_the_category()) as $cat) { echo $cat->cat_name . ','; } ?>,<?php the_tags(); ?>" /><?php } ?><meta http-equiv="Content-Type" content="<?php bloginfo('html_type'); ?>; charset=<?php bloginfo('charset'); ?>" /><meta name="MSSmartTagsPreventParsing" content="true" />
	<style type="text/css" media="screen">
		@import url( <?php bloginfo('stylesheet_url'); ?> );
	</style>
	<link rel="alternate" type="application/rss+xml" title="RSS 2.0" href="<?php bloginfo('rss2_url'); ?>" />
	<link rel="alternate" type="text/xml" title="RSS .92" href="<?php bloginfo('rss_url'); ?>" />
	<link rel="alternate" type="application/atom+xml" title="Atom 0.3" href="<?php bloginfo('atom_url'); ?>" />
	<link rel="pingback" href="<?php bloginfo('pingback_url'); ?>" />
    <?php wp_get_archives('type=monthly&format=link'); ?>
	<?php //comments_popup_script(); // off by default ?>
<script type="text/javascript" src="<?php bloginfo('stylesheet_directory'); ?>/scripts/prototype.lite.js"></script>
<script type="text/javascript" src="<?php bloginfo('stylesheet_directory'); ?>/scripts/moo.fx.js"></script>
<script type="text/javascript" src="<?php bloginfo('stylesheet_directory'); ?>/scripts/moo.fx.pack.js"></script>
<script type="text/javascript">
function init(){
	var stretchers = document.getElementsByClassName('box');
	var toggles = document.getElementsByClassName('tab');
	var myAccordion = new fx.Accordion(
		toggles, stretchers, {opacity: false, height: true, duration: 600}
	);
	//hash functions
	var found = false;
	toggles.each(function(h3, i){
		var div = Element.find(h3, 'nextSibling');
			if (window.location.href.indexOf(h3.title) > 0) {
				myAccordion.showThisHideOpen(div);
				found = true;
			}
		});
		if (!found) myAccordion.showThisHideOpen(stretchers[0]);
}
</script>
	<?php wp_head(); ?>
</head>
<body>
	<div id="contenitore">
		<div id="testata">
			<h1><a href="<?php echo get_option('home'); ?>/"><?php bloginfo('name'); ?></a>
			<div class="description"><?php bloginfo('description'); ?></div></h1>			
		</div>