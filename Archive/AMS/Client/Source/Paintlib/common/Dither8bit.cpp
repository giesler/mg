/*-------------------------------------------------------------------------------------------*\
++                                                                                           ++
++            Copyright (C) 1998 - 2000 by Punk Productions Electronic Entertainment.        ++
++                                http://cust.nol.at/ppee                                    ++
++                                                                                           ++
++     Content: Quantization/Dithering				                                         ++
++  Programmer: Nikolaus Brennig (virtualnik@nol.at)										 ++
++                                                                                           ++
\*-------------------------------------------------------------------------------------------*/
// This code is partially based on:
///////////////////////////////////////////////////////////////////////////
// DIBQuant version 1.0
// Copyright (c) 1993 Edward McCreary.
// All rights reserved.
//
// Redistribution and use in source and binary forms are freely permitted
// provided that the above copyright notice and attibution and date of work
// and this paragraph are duplicated in all such forms.
// THIS SOFTWARE IS PROVIDED "AS IS" AND WITHOUT ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
// WARRANTIES OF MERCHANTIBILILTY AND FITNESS FOR A PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////
#include "stdpch.h"
#include "Dither8bit.h"
#include "NeuQuant.h"

//---------------------------------------------------------------------------------------------
// default palette...
//---------------------------------------------------------------------------------------------
int def_pal[] = 
{
	0,0,0,
	191,0,0,
	0,191,0,
	191,191,0,
	0,0,191,
	191,0,191,
	0,191,191,
	192,192,192,
	192,220,192,
	166,202,240,
	128,0,0,
	0,128,0,
	128,128,0,
	0,0,128,
	128,0,128,
	0,128,128,
	0,81,0,
	81,81,0,
	133,81,0,
	177,81,0,
	217,81,0,
	255,81,0,
	0,133,0,
	81,133,0,
	133,133,0,
	177,133,0,
	217,133,0,
	255,133,0,
	0,177,0,
	81,177,0,
	133,177,0,
	177,177,0,
	217,177,0,
	255,177,0,
	0,217,0,
	81,217,0,
	133,217,0,
	177,217,0,
	217,217,0,
	255,217,0,
	0,255,0,
	81,255,0,
	133,255,0,
	177,255,0,
	217,255,0,
	255,255,0,
	0,0,81,
	81,0,81,
	133,0,81,
	177,0,81,
	217,0,81,
	255,0,81,
	0,81,81,
	81,81,81,
	133,81,81,
	177,81,81,
	217,81,81,
	255,81,81,
	0,133,81,
	81,133,81,
	133,133,81,
	177,133,81,
	217,133,81,
	55,133,81,
	0,177,81,
	81,177,81,
	133,177,81,
	177,177,81,
	217,177,81,
	255,177,81,
	0,217,81,
	81,217,81,
	133,217,81,
	177,217,81,
	217,217,81,
	255,217,81,
	0,255,81,
	81,255,81,
	133,255,81,
	177,255,81,
	217,255,81,
	255,255,81,
	0,0,133,
	81,0,133,
	133,0,133,
	177,0,133,
	217,0,133,
	255,0,133,
	0,81,133,
	81,81,133,
	133,81,133,
	177,81,133,
	217,81,133,
	255,81,133,
	0,133,133,
	81,133,133,
	133,133,133,
	177,133,133,
	217,133,133,
	255,133,133,
	0,177,133,
	81,177,133,
	133,177,133,
	177,177,133,
	217,177,133,
	255,177,133,
	0,217,133,
	81,217,133,
	133,217,133,
	177,217,133,
	217,217,133,
	255,217,133,
	0,255,133,
	81,255,133,
	133,255,133,
	177,255,133,
	217,255,133,
	255,255,133,
	0,0,177,
	81,0,177,
	133,0,177,
	177,0,177,
	217,0,177,
	255,0,177,
	0,81,177,
	81,81,177,
	133,81,177,
	177,81,177,
	217,81,177,
	255,81,177,
	0,133,177,
	81,133,177,
	133,133,177,
	177,133,177,
	217,133,177,
	255,133,177,
	0,177,177,
	81,177,177,
	133,177,177,
	177,177,177,
	217,177,177,
	255,177,177,
	0,217,177,
	81,217,177,
	133,217,177,
	177,217,177,
	217,217,177,
	255,217,177,
	0,255,177,
	81,255,177,
	133,255,177,
	177,255,177,
	217,255,177,
	255,255,177,
	0,0,217,
	81,0,217,
	133,0,217,
	177,0,217,
	217,0,217,
	255,0,217,
	0,81,217,
	81,81,217,
	133,81,217,
	177,81,217,
	217,81,217,
	255,81,217,
	0,133,217,
	81,133,217,
	133,133,217,
	177,133,217,
	217,133,217,
	255,133,217,
	0,177,217,
	81,177,217,
	133,177,217,
	177,177,217,
	217,177,217,
	255,177,217,
	0,217,217,
	81,217,217,
	133,217,217,
	177,217,217,
	217,217,217,
	255,217,217,
	0,255,217,
	81,255,217,
	133,255,217,
	177,255,217,
	217,255,217,
	255,255,217,
	0,0,255,
	81,0,255,
	133,0,255,
	177,0,255,
	217,0,255,
	255,0,255,
	0,81,255,
	81,81,255,
	133,81,255,
	177,81,255,
	217,81,255,
	255,81,255,
	0,133,255,
	81,133,255,
	133,133,255,
	177,133,255,
	217,133,255,
	255,133,255,
	0,177,255,
	81,177,255,
	133,177,255,
	177,177,255,
	217,177,255,
	255,177,255,
	0,217,255,
	81,217,255,
	133,217,255,
	177,217,255,
	217,217,255,
	255,217,255,
	0,255,255,
	81,255,255,
	133,255,255,
	177,255,255,
	217,255,255,
	196,196,196,
	199,199,199,
	202,202,202,
	205,205,205,
	208,208,208,
	211,211,211,
	214,214,214,
	217,217,217,
	220,220,220,
	223,223,223,
	226,226,226,
	229,229,229,
	232,232,232,
	235,235,235,
	238,238,238,
	241,241,241,
	244,244,244,
	247,247,247,
	250,250,250,
	253,253,253,
	155,207,207,
	255,251,240,
	160,160,164,
	128,128,128,
	255,0,0,
	0,255,0,
	255,255,0,
	0,0,255,
	255,0,255,
	0,255,255,
	255,255,255
};


//---------------------------------------------------------------------------------------------
// For ijitter dithering...
//---------------------------------------------------------------------------------------------
INT irand[1024] = 
{
	41,35,190,900,737,364,214,686,
	338,912,585,497,753,443,745,491,
	947,678,731,316,647,268,830,153,
	292,94,13,284,262,439,327,222,
	435,274,845,456,67,699,651,678,
	31,259,602,637,265,568,805,543,
	605,724,971,1020,918,1013,581,571,
	531,525,905,778,28,987,686,818,
	288,410,848,750,832,120,822,253,
	530,585,818,1014,670,381,841,476,
	173,335,532,498,324,576,870,208,
	619,708,560,695,50,59,417,34,
	758,34,401,157,737,395,543,986,
	176,714,665,258,953,882,925,73,
	300,640,126,709,153,213,1001,128,
	434,1002,969,204,851,191,103,214,
	959,788,726,126,557,220,654,358,
	387,495,599,73,97,1023,105,143,
	609,717,465,542,669,156,278,370,
	882,742,29,496,900,847,330,631,
	2,215,1000,57,44,339,971,969,
	530,798,51,116,158,780,500,469,
	724,671,212,932,89,638,53,975,
	818,290,756,716,719,467,656,557,
	72,979,399,373,998,473,285,298,
	997,448,503,43,120,129,135,836,
	270,607,80,768,724,865,653,958,
	123,5,21,7,827,563,642,287,
	280,624,146,474,100,340,462,945,
	133,318,361,533,1016,70,106,4,
	918,627,782,985,790,47,359,360,
	724,247,842,586,720,599,872,118,
	762,790,443,785,173,686,292,392,
	377,254,82,219,805,579,485,828,
	244,837,979,472,552,462,523,245,
	453,352,89,573,919,39,906,345,
	630,45,464,706,969,205,616,212,
	73,106,121,805,520,609,320,20,
	945,315,362,165,273,296,449,652,
	214,681,779,135,919,396,303,241,
	21,29,410,661,193,667,225,960,
	638,745,936,922,167,134,706,437,
	596,191,666,999,217,803,465,597,
	912,568,808,465,217,108,673,102,
	94,334,481,48,668,766,985,113,
	415,994,933,226,780,155,692,839,
	869,568,554,838,649,169,898,121,
	378,374,120,706,355,689,550,223,
	986,553,877,318,98,224,662,274,
	820,703,569,422,319,649,350,1009,
	365,526,739,620,808,673,30,288,
	541,459,706,3,831,577,519,900,
	783,276,773,869,283,808,97,969,
	197,743,300,142,838,566,776,732,
	755,168,909,254,958,498,235,113,
	255,160,464,571,117,262,140,382,
	647,120,371,77,976,190,130,702,
	987,962,582,65,299,908,1018,816,
	639,624,752,679,340,902,818,661,
	426,859,360,531,779,230,1020,1013,
	714,702,893,415,905,138,577,27,
	509,184,847,872,758,626,123,20,
	665,973,979,781,240,68,58,948,
	934,870,339,51,267,971,673,16,
	94,588,492,259,844,371,230,517,
	180,305,526,170,173,719,213,176,
	458,295,255,472,925,276,333,1012,
	633,551,345,322,124,924,705,760,
	973,396,391,32,547,612,952,678,
	391,149,76,688,602,141,334,45,
	665,743,317,689,608,734,945,896,
	429,264,321,489,871,577,677,469,
	415,740,792,159,789,578,512,38,
	510,588,977,33,4,915,559,435,
	911,371,851,320,579,650,175,638,
	202,367,213,719,979,417,149,462,
	602,190,869,39,298,1014,775,173,
	929,446,613,934,436,969,960,105,
	818,308,521,300,77,257,399,279,
	854,198,987,157,968,934,728,11,
	648,385,824,97,875,616,530,610,
	1017,596,464,999,881,279,584,888,
	13,658,41,541,390,809,921,882,
	219,116,540,1018,335,567,952,437,
	688,917,855,757,223,128,620,877,
	141,884,985,907,323,869,529,520,
	165,502,121,957,759,491,21,440,
	224,481,864,399,622,828,379,1012,
	859,354,138,394,143,295,860,503,
	741,135,586,315,818,667,865,832,
	900,966,451,433,423,560,74,16,
	750,629,879,515,303,158,618,1007,
	272,336,923,456,385,835,41,40,
	138,246,745,926,327,673,641,584,
	817,108,717,932,670,222,385,675,
	652,920,528,511,922,67,205,719,
	87,455,336,345,703,445,28,39,
	771,40,895,349,905,863,953,585,
	564,78,608,316,741,734,258,408,
	834,690,781,555,950,20,748,443,
	440,303,627,482,849,894,637,29,
	984,388,467,287,513,190,848,619,
	790,982,835,545,899,281,21,280,
	152,299,556,814,907,505,782,476,
	444,496,970,270,573,877,660,561,
	146,116,943,397,181,676,400,469,
	94,874,64,508,384,118,2,331,
	535,107,822,433,289,987,893,90,
	234,626,542,130,653,113,936,140,
	696,94,729,334,943,250,703,944,
	148,884,29,117,485,476,528,600,
	838,474,498,603,641,928,639,604,
	971,285,566,489,841,372,258,341,
	978,172,282,779,759,937,806,803,
	576,91,419,307,441,565,904,104,
	941,737,554,469,434,818,605,266,
	741,90,220,745,887,349,491,693,
	361,197,58,364,659,664,781,855,
	1003,647,922,991,516,616,434,930,
	725,230,932,198,188,119,351,397,
	707,655,726,298,545,788,425,724,
	260,17,513,792,653,430,699,883,
	28,608,202,32,975,861,470,47,
	69,339,553,983,424,345,460,269,
	1002,550,237,597,590,384,900,985,
	43,504,55,696,749,213,634,672,
	348,78,1018,415,33,508,316,566,
	901,398,385,944,637,959,494,177,
};

INT uranx[1024] = 
{
	6,3,8,0,6,7,6,8,
	0,4,1,6,8,8,5,0,
	4,1,8,0,0,7,2,4,
	0,6,3,2,2,3,0,3,
	0,6,1,0,7,0,6,2,
	7,7,2,5,2,6,7,6,
	2,0,7,6,1,8,6,7,
	3,6,4,0,0,1,5,2,
	0,2,8,8,5,4,5,4,
	5,3,7,8,6,3,2,4,
	1,5,3,4,5,2,8,6,
	7,7,5,5,1,6,7,0,
	0,5,4,2,6,0,3,0,
	6,8,2,4,2,0,8,2,
	7,2,0,2,7,7,2,0,
	2,1,6,8,3,5,2,8,
	8,2,0,4,8,4,7,4,
	6,4,0,6,8,5,8,6,
	0,7,7,8,8,8,8,0,
	5,2,6,3,0,2,4,3,
	8,7,8,7,1,3,6,3,
	7,6,5,4,7,8,4,6,
	1,5,2,3,2,7,3,4,
	6,3,5,8,7,1,0,5,
	1,0,0,7,1,5,7,2,
	4,5,3,4,7,1,4,8,
	0,5,0,4,8,6,8,6,
	3,8,3,8,1,6,4,7,
	7,2,7,7,4,7,0,7,
	1,8,7,1,8,2,0,2,
	2,7,7,2,3,8,7,5,
	5,2,8,2,3,5,4,3,
	0,3,3,2,0,6,2,3,
	1,5,3,3,2,1,3,4,
	3,5,3,4,6,4,4,1,
	2,6,1,3,2,0,4,6,
	7,7,8,5,4,0,0,4,
	8,3,4,5,7,7,2,2,
	1,7,4,2,2,6,0,4,
	1,6,0,3,1,5,1,7,
	3,1,6,2,6,5,4,7,
	6,2,5,6,4,1,0,5,
	1,7,6,3,3,8,0,6,
	2,3,4,2,6,7,0,1,
	8,1,7,5,4,8,2,7,
	0,8,1,2,3,3,5,1,
	0,3,6,7,1,2,5,6,
	3,2,5,3,2,1,5,0,
	4,2,4,0,3,0,2,0,
	1,3,6,0,4,1,1,6,
	3,8,2,3,8,8,1,5,
	3,6,0,7,3,0,5,7,
	2,1,7,8,2,3,2,0,
	5,3,4,4,7,6,6,0,
	7,3,1,0,8,2,0,3,
	2,0,0,6,5,8,4,4,
	2,3,2,8,8,7,1,3,
	5,7,1,3,2,8,2,0,
	0,7,7,7,3,2,5,0,
	7,5,3,2,3,4,7,6,
	7,5,7,1,0,1,5,4,
	5,5,8,3,0,7,0,3,
	5,4,2,3,5,3,0,7,
	8,6,1,6,2,8,2,4,
	8,4,6,1,4,8,5,0,
	0,3,4,1,0,2,0,8,
	2,3,8,8,8,7,6,5,
	4,6,0,0,7,8,2,1,
	6,3,0,3,8,8,7,5,
	2,2,1,3,2,3,2,4,
	4,5,5,1,4,6,8,3,
	4,0,2,5,4,7,4,1,
	3,1,2,1,1,0,2,4,
	6,7,3,7,6,8,7,6,
	0,2,0,1,4,2,1,3,
	5,8,5,3,6,1,4,6,
	0,2,3,8,8,6,5,4,
	5,1,5,8,3,1,7,5,
	4,4,8,5,3,7,5,4,
	7,8,0,6,1,3,7,0,
	6,3,5,1,8,8,5,0,
	4,0,2,4,3,7,3,6,
	5,2,7,6,2,5,4,7,
	1,1,5,6,1,0,0,4,
	6,0,1,2,0,2,2,5,
	5,2,8,0,5,0,3,0,
	7,8,4,1,4,6,2,2,
	2,1,1,3,3,5,6,2,
	0,8,2,5,8,4,2,7,
	4,7,7,4,1,6,1,2,
	7,3,6,8,2,4,2,0,
	1,1,5,1,2,3,3,7,
	8,5,0,5,3,2,5,1,
	3,3,5,3,8,7,4,1,
	3,7,1,8,8,0,7,1,
	6,4,0,0,2,0,6,2,
	4,5,6,5,1,4,7,2,
	6,8,5,2,0,4,6,4,
	8,3,4,1,5,7,0,5,
	7,5,1,1,8,0,2,0,
	7,5,4,1,0,6,0,5,
	8,2,2,8,4,3,6,1,
	1,0,3,4,5,7,4,4,
	2,7,1,7,7,6,5,3,
	8,1,5,6,7,7,7,6,
	3,1,0,0,1,6,5,8,
	7,8,4,6,1,2,4,6,
	0,4,8,0,3,6,1,4,
	2,8,2,4,0,3,1,0,
	4,5,3,1,2,4,5,4,
	4,2,5,7,5,5,1,3,
	8,8,3,0,1,3,5,6,
	5,4,5,8,6,1,7,3,
	4,3,7,1,5,4,4,7,
	6,0,3,7,1,7,8,5,
	8,5,3,0,5,2,5,3,
	3,4,6,5,4,8,5,0,
	8,3,6,4,2,7,5,4,
	1,3,4,5,1,3,1,3,
	7,5,8,1,2,7,5,7,
	4,4,0,8,1,4,3,6,
	0,4,4,7,2,4,8,3,
	5,3,6,8,6,2,1,6,
	5,5,0,4,6,5,1,2,
	2,0,5,1,4,3,7,6,
	3,3,5,0,6,4,4,3,
	4,1,6,7,0,8,1,6,
	0,6,5,3,2,8,8,8,
};

INT urany[1024] = 
{
	8,8,0,8,-16,0,16,8,
	-8,-16,0,0,0,0,-8,-16,
	0,8,-16,-8,8,0,0,-8,
	-16,0,0,8,8,0,0,-8,
	16,-8,-8,-8,16,0,0,0,
	16,0,8,-8,0,16,8,0,
	-8,-8,0,-16,8,16,8,8,
	0,-16,8,16,-8,0,-8,-8,
	-8,-8,8,8,0,16,-8,-8,
	16,-8,8,0,-16,0,0,-8,
	0,-16,16,-8,-8,16,8,8,
	0,16,0,-16,8,0,-16,0,
	0,-8,0,-16,16,16,-8,8,
	0,-8,8,-8,16,-8,8,-16,
	8,16,16,0,8,0,-8,0,
	0,0,-8,-8,-8,-8,-8,-8,
	-8,8,-8,-8,8,8,8,-8,
	-8,0,-8,-16,-8,-8,8,16,
	8,-16,-16,8,0,8,0,-8,
	8,8,-8,8,8,8,0,8,
	-8,0,8,-8,8,0,-8,0,
	0,8,16,0,8,-8,-8,0,
	16,0,-16,-8,8,-8,8,8,
	16,0,0,8,-8,16,-8,-8,
	0,-16,0,8,0,8,-8,16,
	0,8,-16,0,8,-8,8,8,
	-8,0,8,8,0,8,-16,-16,
	-16,-16,0,-8,8,-16,-8,16,
	8,-16,0,8,-8,-8,0,-8,
	0,0,-16,0,-8,0,16,-16,
	16,-8,8,-16,-8,8,-8,0,
	16,0,8,0,0,0,16,-8,
	0,-16,-8,0,-8,8,16,8,
	0,-16,0,-16,-8,8,-16,8,
	16,0,8,-8,-8,0,-8,8,
	0,-8,-16,8,16,-16,16,0,
	-8,8,-8,-8,0,16,0,-8,
	-16,8,8,-16,0,-16,-16,-8,
	8,0,0,16,8,-8,0,-16,
	0,8,-16,-8,8,16,16,16,
	-8,0,8,0,8,0,8,-8,
	-8,8,-8,-8,-8,-8,-8,0,
	-16,0,0,0,8,-8,-8,0,
	16,-8,-16,8,0,8,-8,16,
	-16,-16,16,8,0,-8,0,-16,
	16,-16,-8,16,-8,0,0,-16,
	0,0,8,-8,16,0,-8,8,
	8,-8,8,8,0,8,0,0,
	-8,-16,8,-8,16,16,8,-8,
	16,-8,8,-8,16,8,8,-16,
	8,0,8,8,-8,-8,-8,0,
	0,-8,-8,-8,-8,0,0,-8,
	-16,-8,8,8,0,-16,-8,8,
	-8,-8,0,-8,-16,-16,0,-16,
	-8,8,-8,0,-8,-16,8,-8,
	0,-16,0,0,-8,8,8,-8,
	-8,8,16,-8,-8,0,16,0,
	0,8,0,-8,8,-16,0,16,
	8,-8,8,0,8,8,-8,-16,
	0,0,-8,8,-8,8,0,16,
	-8,8,0,0,0,-8,0,0,
	-8,-16,16,0,8,-16,8,0,
	16,-16,-8,8,0,16,8,-8,
	-8,0,8,16,8,16,8,0,
	-16,0,8,8,0,0,8,-8,
	-8,0,-8,8,-8,16,8,-8,
	-8,0,0,16,-8,0,16,8,
	8,16,8,-8,8,16,8,-8,
	8,0,8,-8,8,0,8,-16,
	0,-8,-16,8,-8,8,-16,0,
	8,0,8,-8,8,8,16,0,
	8,16,-16,-8,0,-16,-8,0,
	8,8,0,8,16,0,8,0,
	-8,-8,0,-16,8,-8,16,8,
	0,-8,-16,0,8,-16,16,8,
	-8,8,16,8,8,16,8,0,
	8,0,8,0,8,0,-8,0,
	8,-16,0,0,8,0,0,8,
	0,8,-16,0,-16,0,0,-8,
	-8,8,-16,16,8,16,8,0,
	-8,0,-8,16,0,-8,0,16,
	-8,8,16,0,-8,0,16,-8,
	0,-16,0,0,0,0,16,-8,
	0,-8,0,16,0,0,16,-16,
	-8,-8,-8,0,-8,0,8,16,
	0,8,8,16,-8,-8,-8,-16,
	8,0,8,16,8,-16,0,8,
	8,8,-8,-16,0,0,8,-8,
	8,0,-8,-8,-16,8,0,8,
	0,0,16,-8,8,16,-8,-8,
	-16,-16,16,-16,0,0,-8,8,
	16,8,-8,0,0,8,-16,-8,
	-16,8,16,-8,8,16,16,16,
	16,0,-8,-8,16,-8,-8,-8,
	-8,8,0,0,0,-16,-16,-8,
	-8,-8,-8,-16,16,-8,-8,0,
	-8,8,16,0,-16,8,0,-16,
	-16,8,-16,8,0,-16,-8,-16,
	0,8,-8,0,8,16,8,0,
	-16,8,0,0,-8,-16,8,-8,
	0,16,-8,0,8,8,-8,8,
	-8,-8,8,-8,8,8,0,-8,
	16,-8,8,16,0,-8,16,-8,
	8,0,8,-8,-16,-16,-8,8,
	8,16,-8,8,-8,8,8,0,
	-16,-8,16,-8,0,16,8,16,
	16,-8,8,0,-8,0,8,8,
	8,16,16,-8,8,0,-8,0,
	0,0,8,-8,8,8,-8,8,
	0,-8,16,8,8,8,-8,8,
	-16,8,-16,-8,-16,8,0,8,
	0,0,16,-16,-16,16,0,0,
	8,16,-8,-16,-16,-16,-8,-16,
	-8,8,-8,-16,8,-8,-16,0,
	-8,-16,-8,16,-8,-8,8,8,
	-8,0,16,-8,-8,16,0,-8,
	-8,-16,-16,-8,0,8,0,-16,
	8,8,-8,-8,-8,-8,-8,8,
	8,-8,16,8,-16,-8,8,0,
	-8,-8,8,8,8,8,8,8,
	8,0,-8,-16,-8,-8,-16,-8,
	-8,16,-8,0,-8,8,16,0,
	0,-8,-16,0,8,8,8,-16,
	0,16,-8,8,-8,16,8,8,
	16,0,8,-8,8,0,8,-16,
	-16,0,16,16,16,8,-8,0,
	16,-8,-8,16,0,-8,8,0,
	0,-8,0,-8,-8,-16,-8,-8,
};

//---------------------------------------------------------------------------------------------
// ordered dither matrix
//---------------------------------------------------------------------------------------------
const INT DitherLUT[4][4] = { -7, 1, -5, 3, 5, -3, 7, -1, -4, 4, -6, 2, 8, 0, 6, -2 };
LONG SQR_Red[256];
LONG SQR_Grn[256];
LONG SQR_Blu[256];



//---------------------------------------------------------------------------------------------
// A look up table to speed up getNearestColor...
//---------------------------------------------------------------------------------------------
void CreateSQRLUT()
{
        for( int i=0; i<256; i++ ) 
        {
                SQR_Red[i] = (long)i*(long)i*(long)297;
                SQR_Grn[i] = (long)i*(long)i*(long)590;
                SQR_Blu[i] = (long)i*(long)i*(long)113;
        }
}


//---------------------------------------------------------------------------------------------
// Returns the index for the best RGBmatch in the given palette for the given RGBValues.
//---------------------------------------------------------------------------------------------
int getNearestColor( BYTE *p_Palr, BYTE *p_Palg, BYTE *p_Palb, int dwNumEntries, 
                                         BYTE bRed, BYTE bGreen, BYTE bBlue )
{
        int nMinDist = 0x7FFFFFFF;
        int nMinIndex = 0;
        int     n;
        int nDist;

        for( int i=0; i<dwNumEntries; i++, p_Palr++, p_Palg++, p_Palb++ )
        {
                // Die Reihenfolge (GRB) der folgenden Tests ist entscheidend
                // (nach Gewichtung sortiert) und darf nicht geändert werden.
                n = *p_Palg-bGreen;
                nDist = SQR_Grn[abs(n)];
                if( nDist >= nMinDist ) continue;

                n = *p_Palr-bRed;
                nDist += SQR_Red[abs(n)];
                if( nDist >= nMinDist ) continue;

                n = *p_Palb-bBlue;
                nDist += SQR_Blu[abs(n)];
                if( nDist >= nMinDist ) continue;

                if( nDist == 0 ) return i;
                nMinDist = nDist;
                nMinIndex = i;
        }

        return nMinIndex;
}


//---------------------------------------------------------------------------------------------
// Neural Network Quantization (24bit -> 8bit):
//---------------------------------------------------------------------------------------------
BYTE *DitherPicture8bitNeuQuant( BYTE *MyBuffer, INT W, INT H, BYTE palette[768], 
                                                                 int PaletteType, int DitherType )
{
        int Factor = 0;
        switch( PaletteType )
        {
                case IDX_NEUQUANTBEST:   Factor = 1;  break;
                case IDX_NEUQUANTMIDDLE: Factor = 10; break;
                case IDX_NEUQUANTWORST:  Factor = 30; break;
                default: return 0; break;
        }
        initnet( MyBuffer, WIDTHBYTES(32*W)*H, Factor );
        learn();
        unbiasnet();

        // get the computed colormap...
        getcolourmap( palette );
        BYTE Palr[256], Palg[256], Palb[256];
        if( DitherType == IDX_FS )
        {
                for( int i=0; i<256; i++ )
                {
                        int tmp = i*3;
                        Palr[i] = palette[tmp+0];
                        Palg[i] = palette[tmp+1];
                        Palb[i] = palette[tmp+2];
                }
        }

        // Build the indexes...
        inxbuild();

        // Alloc our 8bit Buffer...
        BYTE *DestBuffer = new BYTE[WIDTHBYTES(8*W)*H*sizeof(BYTE)];

        // Now dither the picture, plus add the best colormapindex to the 8bit buffer...
        int                     Width24bit, Width8bit;
        INT                     Row24 = 0;
        INT                     Row08 = 0;
        int                     x, y;

        Width24bit = WIDTHBYTES(32*W);
        Width8bit = WIDTHBYTES(8*W); 
        
        // FS:
        // Folgendes wird später gebraucht (mußt Du passend intialisieren)
        BYTE * p_src = MyBuffer;
        BYTE * p_dst = DestBuffer;

        DWORD srcPitch = Width24bit;
        DWORD dstPitch = Width8bit;

        LONG srcIncrement = 4;
        LONG dstIncrement = 1;

        DWORD srcRightOfs = (W * 4) - 4;
        DWORD dstRightOfs = (W * 1) - 1;

        // Die folgende Variablen werden temporär bentutzt:
        float r, g, b, palR, palG, palB, error, delta;
        int index;
        LONG srcInc, dstInc;
        BYTE * p_srcBits, *p_dstBits;
        float * p_thisRow, *p_nextRow;
        float * p_oddErrors;
        float * p_evenErrors;

        if( DitherType == IDX_FS )
        {
                // SQR LookUpTable machen...
                CreateSQRLUT();

                // Speicher für Fehler reservieren
                p_oddErrors = new float[(W + 2) * 6];
                p_evenErrors = &p_oddErrors[(W + 2) * 3];
                memset(p_oddErrors, 0, (W + 2) * 6 * sizeof(float));
        }

        // Alle Zeilen dithern
        for( y = 0; y < H; y++ )
        {
                if( DitherType == IDX_FS )
                {
                        if( y&1 ) // ungerade (odd) Zeile
                        {
                                // Zuerst einmal besorgen wir uns zwei temporäre
                                // Zeiger auf das Ende der beiden aktuellen Zeilen
                                // im Bild.
                                p_srcBits = (BYTE *) p_src + srcRightOfs;
                                p_dstBits = (BYTE *) p_dst + dstRightOfs;

                                // Increment-Werte pro Pixel entsprechend kopieren
                                srcInc = -srcIncrement;
                                dstInc = -dstIncrement;
        
                                // Die aktuelle Zeile immer von links nach rechts, und
                                // die nächste Zeile immer von rechts nach links
                                p_thisRow  = p_oddErrors + 3;
                                p_nextRow = p_evenErrors + (W * 3);
                        }
                        else // gerade (even) Zeile
                        {
                                // Zuerst einmal besorgen wir uns zwei temporäre
                                // Zeiger auf das Ende der beiden aktuellen Zeilen
                                // im Bild.
                                p_srcBits = p_src;
                                p_dstBits = p_dst;

                                // Increment-Werte pro Pixel entsprechend kopieren
                                srcInc = +srcIncrement;
                                dstInc = +dstIncrement;

                                // Die aktuelle Zeile immer von links nach rechts, und
                                // die nächste Zeile immer von rechts nach links
                                p_thisRow  = p_evenErrors + 4;
                                p_nextRow = p_oddErrors + (W * 4);
                        }

                        // Die ersten Fehlerwere für die nächste Zeile müssen initialisiert werden:
                        p_nextRow[0] = 0;
                        p_nextRow[1] = 0;
                        p_nextRow[2] = 0;
                }
                else
                {
                        // Zuerst einmal besorgen wir uns zwei temporäre
                        // Zeiger auf das Ende der beiden aktuellen Zeilen
                        // im Bild.
                        p_srcBits = p_src;
                        p_dstBits = p_dst;

                        // Increment-Werte pro Pixel entsprechend kopieren
                        srcInc = +srcIncrement;
                        dstInc = +dstIncrement;
                }

                // Und hier fängt die X-Schleife los. Im moment enthält die nur
                // Pseudo-Code (in "/* ... */"
                for( x=0; x<W; x++ )
                {
                        // Pixel schreiben und p_src zum nächsten Pixel
                        b = p_srcBits[0];
                        g = p_srcBits[1];
                        r = p_srcBits[2];
                        p_srcBits += srcInc;

                        // Nun die alten Fehlerwerte zu den Komponenten hinzuaddieren
                        switch( DitherType )
                        {
                                case IDX_ORDERED:
                                        r += (DitherLUT[x&3][y&3]<<1); r = CLIP(r); 
                                        g += (DitherLUT[x&3][y&3]<<1); g = CLIP(g); 
                                        b += (DitherLUT[x&3][y&3]<<1); b = CLIP(b); 
                                        break;

                                case IDX_FS:
                                        r += p_thisRow[0]; r = CLIP(r);
                                        g += p_thisRow[1]; g = CLIP(g);
                                        b += p_thisRow[2]; b = CLIP(b);
                                        break;
                        }

                        // Jetzt ermitteln wir die passende Farbe aus der Palette
                        if( DitherType == IDX_FS )
                        {
                                index = getNearestColor( Palr, Palg, Palb, 256, (BYTE)r, (BYTE)g, (BYTE)b );
                        }
                        else
                        {
                                index = inxsearch( (BYTE)b, (BYTE)g, (BYTE)r );
                        }

                        // Berechnen wir uns die nächsten Pixel Fehler, nur FS Dithering.
                        if( DitherType == IDX_FS )
                        {
                                int tmpIndex = index * 3;
                                palR = (float) palette[tmpIndex+0];
                                palG = (float) palette[tmpIndex+1];
                                palB = (float) palette[tmpIndex+2];

                                // Rot: neuen Fehler berechnen und verteilen
                                delta = error = (r - palR) / 16.0f;
                                delta += error;
                                p_nextRow[-3]  = error; error += delta;
                                p_nextRow[+3] += error; error += delta;
                                p_nextRow[+0] += error; error += delta;
                                p_thisRow[+3] += error;

                                // Grün: neuen Fehler berechnen und verteilen
                                delta = error = (g - palG) / 16.0f;
                                delta += error;
                                p_nextRow[-2]  = error; error += delta;
                                p_nextRow[+4] += error; error += delta;
                                p_nextRow[+1] += error; error += delta;
                                p_thisRow[+4] += error;

                                // Blau: neuen Fehler berechnen und verteilen
                                delta = error = (b - palB) / 16.0f;
                                delta += error;
                                p_nextRow[-1]  = error; error += delta;
                                p_nextRow[+5] += error; error += delta;
                                p_nextRow[+2] += error; error += delta;
                                p_thisRow[+5] += error;

                                // Fehler-Pointer anpassen
                                p_nextRow -= 3;
                                p_thisRow += 3;
                        }

                        // Pixel schreiben und p_dst zum nächsten Pixel
                        *p_dstBits = index;
                        p_dstBits += dstInc;
                }

                // Pitch aufaddieren
                p_src = (BYTE *)p_src + srcPitch;
                p_dst = (BYTE *)p_dst + dstPitch;

//                HandleWindowMessages();
        }

        // Speicher wieder freigeben
        if( DitherType == IDX_FS ) delete [] p_oddErrors;

        // Free the 24bit Buffer...
        //delete [] MyBuffer;

        // Return the 8bit Buffer now...
        return DestBuffer;
}



///////////////////////////////////////////////////////////////////////////
// DIBQuant version 1.0
// Copyright (c) 1993 Edward McCreary.
// All rights reserved.
//
// Redistribution and use in source and binary forms are freely permitted
// provided that the above copyright notice and attibution and date of work
// and this paragraph are duplicated in all such forms.
// THIS SOFTWARE IS PROVIDED "AS IS" AND WITHOUT ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, WITHOUT LIMITATION, THE IMPLIED
// WARRANTIES OF MERCHANTIBILILTY AND FITNESS FOR A PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////


//---------------------------------------------------------------------------------------------
// build buffer space and lookup table
//---------------------------------------------------------------------------------------------
LPQUBUF InitLUT()
{
        LPQUBUF                 lpBuffer;
        unsigned long   i; 
        unsigned long   len;
 
        lpBuffer = (LPQUBUF)malloc(sizeof(QUBUF));
        if(!lpBuffer) return NULL;
  
        // lut for x^2
        for( i=0; i<256; i++ ) lpBuffer->SQR[i] = (long)i*(long)i;
 
        len = (long)COLOR_MAX*(long)COLOR_MAX*(long)COLOR_MAX;
 
        // buffer to store every color in image
        //lpBuffer->lpHisto = (LPNode *)GlobalAllocPtr(GHND,len*sizeof(LPNode));
		//lpBuffer->lpHisto = (LPNode*)malloc(len*sizeof(LPNode));
		lpBuffer->lpHisto = (LPNode *)GlobalAlloc(GPTR,len*sizeof(LPNode));

        return lpBuffer;
}


//---------------------------------------------------------------------------------------------
// free memory associated with buffers and luts
//---------------------------------------------------------------------------------------------
VOID ClearLUT( LPQUBUF lpBuffers )
{
        unsigned long len = (long)COLOR_MAX*(long)COLOR_MAX*(long)COLOR_MAX;
        unsigned long i;
 
        if(lpBuffers->lpHisto)
        {
                for(i=0;i<len;i++)
                        if(lpBuffers->lpHisto[i] != NULL)
                        {
                                delete lpBuffers->lpHisto[i];
                                lpBuffers->lpHisto[i] = NULL;
                        } 
                //free((LPSTR)lpBuffers->lpHisto);  
				GlobalFree((LPSTR)lpBuffers->lpHisto);
        }
        
        delete lpBuffers;
}


//---------------------------------------------------------------------------------------------
// main procedures, quantize 24-bit dib
//---------------------------------------------------------------------------------------------
BYTE *DitherPicture8bit( BYTE *MyBuffer, INT PaletteType, INT DitherType, INT W, INT H, 
                                                 UCHAR palette[768] )
{
        long                    width,height;
        BYTE                    *lpLine;
        long                    i,j;
        int                             r,g,b;
        long                    dib_width;
        BYTE                    *DitherBuffer;
        LPQUBUF                 lpBuffer;
 

        // Check if NeuQuant Method is wanted...
        if( PaletteType == IDX_NEUQUANTBEST
        ||      PaletteType == IDX_NEUQUANTMIDDLE
        ||      PaletteType == IDX_NEUQUANTWORST )
        {
                return DitherPicture8bitNeuQuant( MyBuffer, W, H, palette, PaletteType, DitherType );
        }

        // allocate buffers...
        lpBuffer = InitLUT();
        if(!lpBuffer) return NULL;
 
        // store handle to status indicator callback
        lpBuffer->lpStatus = 0;
  
        width = W;
        height = H;
        dib_width = WIDTHBYTES(32*width);

        // generate list of all colors if needed...
        INT OurRow = 0;
        if(PaletteType != IDX_DEFAULT)      
        {
                for(j=0;j<height;j++)
                {
                        lpLine = MyBuffer + OurRow;
                        i = width;
                        while(i--)
                        {
                                b = (int)*lpLine++;    
                                g = (int)*lpLine++;    
                                r = (int)*lpLine++;    
     
                                add_color( r>>3, g>>3, b>>3, 1L, lpBuffer );
                        }  
                        OurRow += dib_width;
                }
        }
 
        // Which kind of palette quantization is prefered?
        switch(PaletteType)
        {
                case IDX_MEDIAN:                m_box(lpBuffer);                        break;
                case IDX_POPULARITY:    pop(lpBuffer);                          break;
                case IDX_DEFAULT:               LoadDefaultPal(lpBuffer);       break;
                default:                                m_box(lpBuffer);                        break;  
        }
 
        // Now dither the buffer...
        DitherBuffer = Dither( MyBuffer, DitherType, lpBuffer, W, H );

        // Fill our palette with the colors...
        for( i=0; i<256; i++ )
        {
                INT tmp = i*3;
                palette[tmp + 0] = lpBuffer->red[i];
                palette[tmp + 1] = lpBuffer->green[i];
                palette[tmp + 2] = lpBuffer->blue[i]; 
        }
 
        // clean up...
        ClearLUT(lpBuffer);
 
        // Now return the dithered buffer...
        return DitherBuffer; 
}


//---------------------------------------------------------------------------------------------
// Does the actual dithering of the buffer...
//---------------------------------------------------------------------------------------------
BYTE *Dither( BYTE *lpDIB, INT DitherType, LPQUBUF lpBuffer, INT W, INT H )
{
        BYTE                            *lpNewDIB;
        long                            src_dib_width,dest_dib_width;
        long                            width,height;
        BYTE                            *lpSrcLine;
        BYTE                            *lpDestLine;
        long                            i,j;
 
        // alloc memory...
        lpNewDIB = new BYTE[WIDTHBYTES(W<<4)*H];
        if(!lpNewDIB) return NULL; 

        // Transfer values...
        width = W;
        height = H;
        src_dib_width = WIDTHBYTES(32*W);
        dest_dib_width = WIDTHBYTES(8*W); 

        // FS:
        // Folgendes wird später gebraucht (mußt Du passend intialisieren)
        BYTE * p_src = lpDIB;
        BYTE * p_dst = lpNewDIB;

        DWORD srcPitch = src_dib_width;
        DWORD dstPitch = dest_dib_width;

        LONG srcIncrement = 4;
        LONG dstIncrement = 1;

        DWORD srcRightOfs = (W * 4) - 4;
        DWORD dstRightOfs = (W * 1) - 1;

        // Die folgende Variablen werden temporär bentutzt:
        float r, g, b, palR, palG, palB, error, delta;
        int index;
        LONG srcInc, dstInc;
        float * p_thisRow, *p_nextRow;
        float * p_oddErrors;
        float * p_evenErrors;

        if( DitherType == IDX_FS )
        {
                // Speicher für Fehler reservieren
                p_oddErrors = new float[(W + 2) * 6];
                p_evenErrors = &p_oddErrors[(W + 2) * 3];
                memset(p_oddErrors, 0, (W + 2) * 6 * sizeof(float));
        }
  
        // Loop row by row, col by col, and of course dither...
        INT Row24 = 0;
        INT Row08 = 0;
        for( j=0; j<height; j++ )
        {
                if( DitherType == IDX_FS )
                {
                        if( j&1 ) // ungerade (odd) Zeile
                        {
                                lpSrcLine = (BYTE *) p_src + srcRightOfs;
                                lpDestLine = (BYTE *) p_dst + dstRightOfs;

                                srcInc = -srcIncrement;
                                dstInc = -dstIncrement;
        
                                p_thisRow  = p_oddErrors + 3;
                                p_nextRow = p_evenErrors + (W * 3);
                        }
                        else // gerade (even) Zeile
                        {
                                lpSrcLine = p_src;
                                lpDestLine = p_dst;

                                srcInc = +srcIncrement;
                                dstInc = +dstIncrement;

                                p_thisRow  = p_evenErrors + 3;
                                p_nextRow = p_oddErrors + (W * 3);
                        }

                        // Die ersten Fehlerwere für die nächste Zeile müssen initialisiert werden:
                        p_nextRow[0] = 0;
                        p_nextRow[1] = 0;
                        p_nextRow[2] = 0;
                }
                else
                {
                        lpSrcLine = p_src;
                        lpDestLine = p_dst;

                        srcInc = +srcIncrement;
                        dstInc = +dstIncrement;
                }
    
                // No go through the whole scanline...
                i = width;
                while(i--)
                {    
                        // Get RGB Values...
                        b = lpSrcLine[0];
                        g = lpSrcLine[1];
                        r = lpSrcLine[2];
                        lpSrcLine += srcInc;
     
                        // Now dither...
                        switch( DitherType )
                        {
                                case IDX_JITTER:
                                        jitter( i, j, (int *)&r, (int *)&g, (int *)&b );
                                        break;
     
                                case IDX_ORDERED:
                                        r += (DitherLUT[i&3][j&3]<<1); r = CLIP(r); 
                                        g += (DitherLUT[i&3][j&3]<<1); g = CLIP(g); 
                                        b += (DitherLUT[i&3][j&3]<<1); b = CLIP(b); 
                                        break;

                                case IDX_FS:
                                        r += p_thisRow[0]; r = CLIP(r);
                                        g += p_thisRow[1]; g = CLIP(g);
                                        b += p_thisRow[2]; b = CLIP(b);
                                        break;
                        }

                        // Now get the best possible paletteindex matching the RGB Vals.
                        index = GetNeighbor( (int)r, (int)g, (int)b, lpBuffer );

                        // Berechnen wir uns die nächsten Pixel Fehler, nur FS Dithering.
                        if( DitherType == IDX_FS )
                        {
                                palR = (float)lpBuffer->red[index];
                                palG = (float)lpBuffer->green[index];
                                palB = (float)lpBuffer->blue[index];

                                // Rot: neuen Fehler berechnen und verteilen
                                delta = error = (r - palR) / 16.0f;
                                delta += error;
                                p_nextRow[-3]  = error; error += delta;
                                p_nextRow[+3] += error; error += delta;
                                p_nextRow[+0] += error; error += delta;
                                p_thisRow[+3] += error;

                                // Grün: neuen Fehler berechnen und verteilen
                                delta = error = (g - palG) / 16.0f;
                                delta += error;
                                p_nextRow[-2]  = error; error += delta;
                                p_nextRow[+4] += error; error += delta;
                                p_nextRow[+1] += error; error += delta;
                                p_thisRow[+4] += error;

                                // Blau: neuen Fehler berechnen und verteilen
                                delta = error = (b - palB) / 16.0f;
                                delta += error;
                                p_nextRow[-1]  = error; error += delta;
                                p_nextRow[+5] += error; error += delta;
                                p_nextRow[+2] += error; error += delta;
                                p_thisRow[+5] += error;

                                // Fehler-Pointer anpassen
                                p_nextRow -= 3;
                                p_thisRow += 3;
                        }

                        // Now set the new value, or the nearest possible...
                        *lpDestLine = (BYTE)index;
                        lpDestLine += dstInc;
                }  

                // Pitch aufaddieren
                p_src = (BYTE *)p_src + srcPitch;
                p_dst = (BYTE *)p_dst + dstPitch;

//                HandleWindowMessages();
        }

        // Speicher wieder freigeben
        if( DitherType == IDX_FS ) delete [] p_oddErrors;
   
        // Free the srcbuffer, no memleaks wanted :)
        //delete [] lpDIB;

        // Return the dithered buffer now...
        return lpNewDIB;
}


//---------------------------------------------------------------------------------------------
// build new palette with median cut algorithm
// I didn't write this, if you know who did, please let me know!
//---------------------------------------------------------------------------------------------
VOID m_box(LPQUBUF lpBuffer)
{
    int     i, j, max, dr, dg, db;    

    /* force the counts in the corners to be zero */
    force( 0,  0,  0, 0L,lpBuffer);
    force(COLOR_MAX-1,  0,  0, 0L,lpBuffer);
    force( 0, COLOR_MAX-1,  0, 0L,lpBuffer);
    force( 0,  0, COLOR_MAX-1, 0L,lpBuffer);
    force(COLOR_MAX-1, COLOR_MAX-1,  0, 0L,lpBuffer);
    force( 0, COLOR_MAX-1, COLOR_MAX-1, 0L,lpBuffer);
    force(COLOR_MAX-1,  0, COLOR_MAX-1, 0L,lpBuffer);
    force(COLOR_MAX-1, COLOR_MAX-1, COLOR_MAX-1, 0L,lpBuffer);

    /* assign the 1st eight boxes to be the corners */
    make_box( 0,  0,  0, 0, 1L,lpBuffer);
    make_box(COLOR_MAX-1,  0,  0, 1, 1L,lpBuffer);
    make_box( 0, COLOR_MAX-1,  0, 2, 1L,lpBuffer);
    make_box( 0,  0, COLOR_MAX-1, 3, 1L,lpBuffer);
    make_box(COLOR_MAX-1, COLOR_MAX-1,  0, 4, 1L,lpBuffer);
    make_box( 0, COLOR_MAX-1, COLOR_MAX-1, 5, 1L,lpBuffer);
    make_box(COLOR_MAX-1,  0, COLOR_MAX-1, 6, 1L,lpBuffer);
    make_box(COLOR_MAX-1, COLOR_MAX-1, COLOR_MAX-1, 7, 1L,lpBuffer);

    /* set up 9th box to hold the rest of the world */
    lpBuffer->box[8].r0 = 0;
    lpBuffer->box[8].r1 = COLOR_MAX-1;
    lpBuffer->box[8].g0 = 0;
    lpBuffer->box[8].g1 = COLOR_MAX-1;
    lpBuffer->box[8].b0 = 0;
    lpBuffer->box[8].b1 = COLOR_MAX-1;
    squeeze(8,lpBuffer);

    /* split the rest of the boxes */

    for(i=9; i<256; i++)    
    {
        /* find biggest box */
        max = 8;
        for( j=8; j<i; j++ )
                {
            if(lpBuffer->box[j].count > lpBuffer->box[max].count) max = j;
                }

        /* decide which side to split the box along, and split it */
        dr = lpBuffer->box[max].r1 - lpBuffer->box[max].r0;
        dg = lpBuffer->box[max].g1 - lpBuffer->box[max].g0;
        db = lpBuffer->box[max].b1 - lpBuffer->box[max].b0;
        lpBuffer->box[i] = lpBuffer->box[max];              /* copy info over */
        if(dr>=dg && dr>=db) 
        {       /* red! */
            if(dr==2) 
            {             /* tight squeeze */
                lpBuffer->box[i].r1 = lpBuffer->box[i].r0;
                lpBuffer->box[max].r0 = lpBuffer->box[max].r1;
            } 
                        else 
            {                /* figure out where to split */
                j = lpBuffer->box[max].rave;
                if(j==lpBuffer->box[max].r1) j--;
                lpBuffer->box[max].r1 = j;
                lpBuffer->box[i].r0 = j+1;
            }
            squeeze(i,lpBuffer);
            squeeze(max,lpBuffer);
        } 
        else if(dg>=db) 
        {       /* green! */
            if(dg==2) 
            {             /* tight squeeze */
                lpBuffer->box[i].g1 = lpBuffer->box[i].g0;
                lpBuffer->box[max].g0 = lpBuffer->box[max].g1;
            } 
            else 
            {                /* figure out where to split */
                j = lpBuffer->box[max].gave;
                if(j==lpBuffer->box[max].g1)
                    j--;
                lpBuffer->box[max].g1 = j;
                lpBuffer->box[i].g0 = j+1;
            }
            squeeze(i,lpBuffer);
            squeeze(max,lpBuffer);
        } 
        else 
        {       /* blue! */
            if(db==2) 
            {             /* tight squeeze */
                lpBuffer->box[i].b1 = lpBuffer->box[i].b0;
                lpBuffer->box[max].b0 = lpBuffer->box[max].b1;
            } 
            else 
            {                /* figure out where to split */
                j = lpBuffer->box[max].bave;
                if(j==lpBuffer->box[max].b1)
                    j--;
                lpBuffer->box[max].b1 = j;
                lpBuffer->box[i].b0 = j+1;
            }
            squeeze(i,lpBuffer);
            squeeze(max,lpBuffer);
        }

    } /* end of i loop, all the boxes are found */

    /* get palette colors for each box */
    for( i=0; i<256; i++ )
    {
         lpBuffer->red[i]   = (lpBuffer->box[i].r0+lpBuffer->box[i].r1)>>1;
         lpBuffer->green[i] = (lpBuffer->box[i].g0+lpBuffer->box[i].g1)>>1;
         lpBuffer->blue[i]  = (lpBuffer->box[i].b0+lpBuffer->box[i].b1)>>1;
    }
    for( i=0; i<256; i++ )
    {
                lpBuffer->red[i]   *= 255;
                lpBuffer->red[i]   /= (COLOR_MAX-1);
                lpBuffer->green[i] *= 255;
                lpBuffer->green[i] /= (COLOR_MAX-1);
                lpBuffer->blue[i]  *= 255;
                lpBuffer->blue[i]  /= (COLOR_MAX-1);
    }
}


//---------------------------------------------------------------------------------------------
// make a 1x1x1 box at index with color rgb count c
//---------------------------------------------------------------------------------------------
VOID make_box(int r, int g, int b, int index, unsigned long c,LPQUBUF lpBuffer)
{
    lpBuffer->box[index].r0             = r;
    lpBuffer->box[index].r1             = r;
    lpBuffer->box[index].g0             = g;
    lpBuffer->box[index].g1             = g;
    lpBuffer->box[index].b0             = b;
    lpBuffer->box[index].b1             = b;
    lpBuffer->box[index].count  = c;
}


//---------------------------------------------------------------------------------------------
// shrink a boxes extremes to fit tightly
// if a box is 1x1x1 change its count to 1
//---------------------------------------------------------------------------------------------
VOID squeeze( int b, LPQUBUF lpBuffer )
{
    int                         r0, r1, g0, g1, b0, b1;
    long                        i, j, k;
    unsigned long       count = 0;
    LPNode                      ptr;
    DWORD                       index;

    r0 = lpBuffer->box[b].r0;
    r1 = lpBuffer->box[b].r1;
    g0 = lpBuffer->box[b].g0;
    g1 = lpBuffer->box[b].g1;
    b0 = lpBuffer->box[b].b0;
    b1 = lpBuffer->box[b].b1;

    lpBuffer->box[b].r0 = COLOR_MAX-1; lpBuffer->box[b].r1 = 0;
    lpBuffer->box[b].g0 = COLOR_MAX-1; lpBuffer->box[b].g1 = 0;
    lpBuffer->box[b].b0 = COLOR_MAX-1; lpBuffer->box[b].b1 = 0;
    lpBuffer->box[b].rave = 0;
    lpBuffer->box[b].gave = 0;
    lpBuffer->box[b].bave = 0;

    for( i=r0; i<=r1; i++ )
        {
        for( j=g0; j<=g1; j++ )
                {
            for( k=b0; k<=b1; k++ )
            {    
                                index = INDEX(i,j,k);
                                ptr = lpBuffer->lpHisto[index];
                                if(ptr) 
                                {
                                        if(ptr->count>0L) 
                                        {
                                                lpBuffer->box[b].r0 = MIN(i, lpBuffer->box[b].r0);
                                                lpBuffer->box[b].r1 = MAX(i, lpBuffer->box[b].r1);
                                                lpBuffer->box[b].g0 = MIN(j, lpBuffer->box[b].g0);
                                                lpBuffer->box[b].g1 = MAX(j, lpBuffer->box[b].g1);
                                                lpBuffer->box[b].b0 = MIN(k, lpBuffer->box[b].b0);
                                                lpBuffer->box[b].b1 = MAX(k, lpBuffer->box[b].b1);
                                                lpBuffer->box[b].rave += (unsigned long)i * (unsigned long)ptr->count;
                                                lpBuffer->box[b].gave += (unsigned long)j * (unsigned long)ptr->count;
                                                lpBuffer->box[b].bave += (unsigned long)k * (unsigned long)ptr->count;
                                                count += (unsigned long)ptr->count;
                                        }
                                }
            }
                }
        }

    /* box is now shrunk */
    if( count )
    {
        lpBuffer->box[b].rave /= count;
        lpBuffer->box[b].gave /= count;
        lpBuffer->box[b].bave /= count;
    }

    lpBuffer->box[b].count = MIN(count, COUNT_LIMIT);

    if( lpBuffer->box[b].r0 == lpBuffer->box[b].r1 &&
        lpBuffer->box[b].g0 == lpBuffer->box[b].g1 &&
                lpBuffer->box[b].b0 == lpBuffer->box[b].b1) 
    {   /* box is min size */
        lpBuffer->box[b].count = 1L;       /* so it won't get split again */
    }
}


//---------------------------------------------------------------------------------------------
// adds a RGB Color into a 8bit colorsheme...
//---------------------------------------------------------------------------------------------
VOID add_color(int r, int g, int b, unsigned long c, LPQUBUF lpBuffer)
{
        LPNode                          ptr;
    unsigned long               ltmp;
    
    DWORD index = INDEX(r,g,b);
    c = MIN(c,COUNT_LIMIT);
    if((ptr = lpBuffer->lpHisto[index]) == NULL)  // new color
    {
                ptr = lpBuffer->lpHisto[index] = new Node[sizeof(Node)];
                ptr->index = -1;
                ptr->count = c;
    }
    else
    {    
                ltmp = ptr->count;
                ltmp += c;
                ptr->count = MIN(ltmp,COUNT_LIMIT);
    }
    
}


//---------------------------------------------------------------------------------------------
// force a color...
//---------------------------------------------------------------------------------------------
VOID force( int r, int g, int b, unsigned long c, LPQUBUF lpBuffer )
{
    LPNode    ptr;
    DWORD     index;
    
    c = MIN(c,COUNT_LIMIT);    
    index = INDEX(r,g,b);
    
    if((ptr = lpBuffer->lpHisto[index]) == NULL)  // new color
    {
                ptr = lpBuffer->lpHisto[index] = new Node[sizeof(Node)];
                ptr->index = -1; 
                ptr->count = 0L;
    }
    
    ptr->count = c;
}


//---------------------------------------------------------------------------------------------
// popularity sort
//---------------------------------------------------------------------------------------------
VOID pop( LPQUBUF lpBuffer )
{
    int             i, r, g,b;
    LPNode          ptr;
    unsigned long   pal[256];
    DWORD           index;
    DWORD           index_cache = (DWORD)-1;
    LPNode                      ptr_cache;
    
    memset( pal, 0, sizeof(unsigned long)*256 );
    
    /* force corners of rgb color cube out of the running */
    add_color( 0,  0,  0, 0L,lpBuffer);
    add_color(COLOR_MAX-1,  0,  0, 0L,lpBuffer);
    add_color( 0, COLOR_MAX-1,  0, 0L,lpBuffer);
    add_color( 0,  0, COLOR_MAX-1, 0L,lpBuffer);
    add_color(COLOR_MAX-1, COLOR_MAX-1,  0, 0L,lpBuffer);
    add_color( 0, COLOR_MAX-1, COLOR_MAX-1, 0L,lpBuffer);
    add_color(COLOR_MAX-1,  0, COLOR_MAX-1, 0L,lpBuffer);
    add_color(COLOR_MAX-1, COLOR_MAX-1, COLOR_MAX-1, 0L,lpBuffer);

    /* force feed the corners into the palette */
    lpBuffer->red[0] = 0;                       lpBuffer->green[0] =  0;                        lpBuffer->blue[0] =  0;
    lpBuffer->red[1] = COLOR_MAX-1; lpBuffer->green[1] =  0;                    lpBuffer->blue[1] =  0;
    lpBuffer->red[2] = 0;                       lpBuffer->green[2] = COLOR_MAX-1;       lpBuffer->blue[2] =  0;
    lpBuffer->red[3] = 0;                       lpBuffer->green[3] =  0;                        lpBuffer->blue[3] = COLOR_MAX-1;
    lpBuffer->red[4] = COLOR_MAX-1; lpBuffer->green[4] = COLOR_MAX-1;   lpBuffer->blue[4] =  0;
    lpBuffer->red[5] = 0;                       lpBuffer->green[5] = COLOR_MAX-1;       lpBuffer->blue[5] = COLOR_MAX-1;
    lpBuffer->red[6] = COLOR_MAX-1; lpBuffer->green[6] =  0;                    lpBuffer->blue[6] = COLOR_MAX-1;
    lpBuffer->red[7] = COLOR_MAX-1; lpBuffer->green[7] = COLOR_MAX-1;   lpBuffer->blue[7] = COLOR_MAX-1;
    
    
    for(r=0; r<COLOR_MAX; r++)
    {
        for(g=0; g<COLOR_MAX; g++) 
                {
            for(b=0; b<COLOR_MAX; b++) 
            {             
                                index = INDEX(r,g,b);
                                if(index == index_cache)
                                {
                                        ptr = ptr_cache;
                                }
                                else
                                {
                                        ptr = lpBuffer->lpHisto[index];
                                        ptr_cache = ptr;
                                        index_cache = index;
                                }
             
                                if(ptr != NULL) 
                                {
                                        if(ptr->count > pal[255]) 
                                        {
                                                pal[255] = ptr->count;
                                                lpBuffer->red[255]       = r;
                                                lpBuffer->green[255] = g;
                                                lpBuffer->blue[255]      = b;

                                                i = 255;        /* bubble up */
                                                while(pal[i]>pal[i-1] && i>8) 
                                                {
                                                        SWAP(pal[i], pal[i-1]);
                                                        SWAP(lpBuffer->red[i], lpBuffer->red[i-1]);
                                                        SWAP(lpBuffer->green[i], lpBuffer->green[i-1]);
                                                        SWAP(lpBuffer->blue[i], lpBuffer->blue[i-1]);
                                                        i--;
                                                }
                                        }
                                }                
                        }       /* end of current chain */
                }       /* end of r loop */            
    }   
        
    for(i=0; i<256; i++)
    {
                lpBuffer->red[i]        *= 255;
                lpBuffer->red[i]        /= (COLOR_MAX-1);
                lpBuffer->green[i]      *= 255;
                lpBuffer->green[i]      /= (COLOR_MAX-1);
                lpBuffer->blue[i]       *= 255;
                lpBuffer->blue[i]       /= (COLOR_MAX-1);
    }
}       /* end of pop */


//---------------------------------------------------------------------------------------------
// Gets the neighbour of a color...
//---------------------------------------------------------------------------------------------
INT GetNeighbor(int r,int g,int b,LPQUBUF lpBuffer)
{
        INT             bReturn; 
        DWORD   index = 0;
        LONG    min_dist = 0;
        LONG    dist = 0;
        INT             c;
        INT             dr,dg,db;
        INT             i,j,k; 
        LPNode  ptr;
  

        index = INDEX((r>>3),(g>>3),(b>>3));
        if( (ptr = lpBuffer->lpHisto[index]) == NULL)
        {
                lpBuffer->lpHisto[index] = ptr = (Node *)malloc(sizeof(Node));
                ptr->count = 0L;
                ptr->index = -1;
        }
  
        if( (bReturn = ptr->index) == -1)
        {   
                ptr->index = 0;
                i = lpBuffer->red[0];
                j = lpBuffer->green[0];
                k = lpBuffer->blue[0];

                dr = i - r;
                dg = j - g;
                db = k - b;
                min_dist = lpBuffer->SQR[abs(dr)] + lpBuffer->SQR[abs(dg)] + lpBuffer->SQR[abs(db)];
   
                for(c = 1;c<256;c++)
                {
                        i = lpBuffer->red[c];
                        j = lpBuffer->green[c];
                        k = lpBuffer->blue[c];
   
                        dr = i - r;
                        dg = j - g;
                        db = k - b;   
                        dist = lpBuffer->SQR[abs(dr)] + lpBuffer->SQR[abs(dg)] + lpBuffer->SQR[abs(db)];
    
                        if(dist < min_dist)
                        {
                                ptr->index = c; 
                                min_dist = dist;
                        }
                }
                bReturn = ptr->index;
        
        }
        return bReturn;
}


//---------------------------------------------------------------------------------------------
// Jitter dithering type...
//---------------------------------------------------------------------------------------------
VOID jitter(long x, long y, int *r,int *g,int *b)
{
        INT p,q;
        INT tmp;
 
        // Red...
        tmp = *r;
        if(tmp < 248)
        {
                p = tmp & 7;
                q = jitterx(x,y,0);
                if(p <= q) tmp += 8;
                q = tmp + jittery(x,y,0);
                if(q >= 0 && q <= 255) tmp = q;
                *r = tmp & 0xF8;
        }
  
        // Green...
        tmp = *g;
        if(tmp < 248)
        {
                p = tmp & 7;
                q = jitterx(x,y,1);
                if(p <= q) tmp += 8;
                q = tmp + jittery(x,y,1);
                if(q >= 0 && q <= 255) tmp = q;
                *g = tmp & 0xF8;
        } 
  
        // Blue...
        tmp = *b;
        if(tmp < 248)
        {
                p = tmp & 7;
                q = jitterx(x,y,2);
                if(p <= q) tmp += 8;
                q = tmp + jittery(x,y,2);
                if(q >= 0 && q <= 255) tmp = q;
                *b = tmp & 0xF8;
        }
}


//---------------------------------------------------------------------------------------------
// Loads the default palette if wanted...
//---------------------------------------------------------------------------------------------
VOID LoadDefaultPal( LPQUBUF lpBuffer )
{
        INT *ptr = def_pal; 
        INT i;
 
        for( i=0; i<256; i++ )
        {
                lpBuffer->red[i]   = *ptr++;
                lpBuffer->green[i] = *ptr++;
                lpBuffer->blue[i]  = *ptr++; 
        }
}


