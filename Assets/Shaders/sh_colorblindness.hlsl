#ifndef COLORBLINDNESS_INCLUDED
#define COLORBLINDNESS_INCLUDED

void ColorblindnessFilter_float(int Type, float Intensity, float4 ColorIn, out float3 OutColor)
{
	float L = (17.8824 * ColorIn.r) + (43.5161 * ColorIn.g) + (4.11935 * ColorIn.b);
	float M = (3.45565 * ColorIn.r) + (27.1554 * ColorIn.g) + (3.86714 * ColorIn.b);
	float S = (0.0299566 * ColorIn.r) + (0.184309 * ColorIn.g) + (1.46709 * ColorIn.b);

	float l, m, s;
	if (Type == 0) //Protanopia
	{
		l = 0.0 * L + 2.02344 * M + -2.52581 * S;
		m = 0.0 * L + 1.0 * M + 0.0 * S;
		s = 0.0 * L + 0.0 * M + 1.0 * S;
	}
	
	if (Type == 1) //Deuteranopia
	{
		l = 1.0 * L + 0.0 * M + 0.0 * S;
    	m = 0.494207 * L + 0.0 * M + 1.24827 * S;
    	s = 0.0 * L + 0.0 * M + 1.0 * S;
	}
	
	if (Type == 2) //Tritanopia
	{
		l = 1.0 * L + 0.0 * M + 0.0 * S;
    	m = 0.0 * L + 1.0 * M + 0.0 * S;
    	s = -0.395913 * L + 0.801109 * M + 0.0 * S;
	}
	
	float4 error;
	error.r = (0.0809444479 * l) + (-0.130504409 * m) + (0.116721066 * s);
	error.g = (-0.0102485335 * l) + (0.0540193266 * m) + (-0.113614708 * s);
	error.b = (-0.000365296938 * l) + (-0.00412161469 * m) + (0.693511405 * s);
	error.a = 1.0;
	float4 diff = ColorIn - error;
	float4 correction;
	correction.r = 0.0;
	correction.g =  (diff.r * 0.7) + (diff.g * 1.0);
	correction.b =  (diff.r * 0.7) + (diff.b * 1.0);
	correction = ColorIn + correction;
	correction.a = ColorIn.a * Intensity;

    OutColor = correction;
}

#endif