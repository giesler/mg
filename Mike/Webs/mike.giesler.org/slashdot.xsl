<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:template match="/">
		<html>
			<head>
				<title>Test Stylesheet</title>
			</head>
			<body>
				<table>
					<xsl:for-each select="backslash/story">
						<tr>
							<td>
								<img height="20" width="20">
									<xsl:attribute name="SRC">
										http://images.slashdot.org/topics/<xsl:value-of select="image"></xsl:value-of>
									</xsl:attribute>
								</img>
							</td>
							<td>
								<a>
									<xsl:attribute name="HREF">
										<xsl:value-of select="url"></xsl:value-of>
									</xsl:attribute>
									<xsl:value-of select="title"></xsl:value-of>
								</a>
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
