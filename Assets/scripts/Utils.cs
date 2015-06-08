﻿using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour {
	public static void RGB2HSV( float r, float g, float b, ref float h, ref float s, ref float v )
	{
		float min, max, delta;
		min = Mathf.Min( r, g, b );
		max = Mathf.Max( r, g, b );
		v = max;				// v
		delta = max - min;
		if( max != 0 )
			s = delta / max;		// s
		else {
			// r = g = b = 0		// s = 0, v is undefined
			s = 0f;
			h = -1f;
			return;
		}
		if( r == max )
			h = ( g - b ) / delta;		// between yellow & magenta
		else if( g == max )
			h = 2 + ( b - r ) / delta;	// between cyan & yellow
		else
			h = 4 + ( r - g ) / delta;	// between magenta & cyan
		h *= 60;				// degrees
		if( h < 0 )
			h += 360f;
	}
	//h: [0, 360)
	//s: [0, 1]
	//v: [0, 1]
	//r,g,b: [0,1]
	public static Color HSVtoRGB(float h, float s, float v, float a, ref float r, ref float g, ref float b )
	{
		int i;
		float f, p, q, t;
		if( s == 0 ) {
			// achromatic (grey)
			r = g = b = v;
			return new Color(r,g,b,a);
		}
		h /= 60f;			// sector 0 to 5
		i = Mathf.FloorToInt( h );
		f = h - i;			// factorial part of h
		p = v * ( 1 - s );
		q = v * ( 1 - s * f );
		t = v * ( 1 - s * ( 1 - f ) );
		switch( i ) {
			case 0:
				r = v;
				g = t;
				b = p;
				break;
			case 1:
				r = q;
				g = v;
				b = p;
				break;
			case 2:
				r = p;
				g = v;
				b = t;
				break;
			case 3:
				r = p;
				g = q;
				b = v;
				break;
			case 4:
				r = t;
				g = p;
				b = v;
				break;
			default:		// case 5:
				r = v;
				g = p;
				b = q;
				break;
		}
		return new Color(r,g,b,a);
	}
}
