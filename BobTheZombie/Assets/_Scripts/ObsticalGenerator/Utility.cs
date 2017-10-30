using System.Collections;
using System.Collections.Generic;

public static class Utility {

	public static T[] RandomlyShuffledArray<T> ( T[] array, int seed ) {
		
		System.Random prng = new System.Random (seed);

		for (int i = 0; i < array.Length - 1; i++) {

			//find random value between index and array length
			int randomIndex = prng.Next ( i, array.Length );

			//swap i and random index
			T tempElement = array [randomIndex];
			array [randomIndex] = array [i];
			array [i] = tempElement;
		}

		return array;
	}
}
