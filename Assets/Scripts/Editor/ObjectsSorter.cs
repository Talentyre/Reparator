namespace RSTools
{
	using UnityEngine;
	using UnityEditor;

	sealed class ObjectsSorter_Menu
	{
		const string SHORTCUT = "%&s";

		[MenuItem ("GameObject/Sort Objects by Name " + SHORTCUT, true)]
		static bool CheckSelectionCount ()
		{
			return Selection.gameObjects.Length > 1;
		}

		[MenuItem ("GameObject/Sort Objects by Name " + SHORTCUT)]
		public static void SortObjectsByName ()
		{
			GameObject[] selection = Selection.gameObjects;
			System.Array.Sort (selection, delegate (GameObject a, GameObject b) { return a.name.CompareTo (b.name); });

			foreach (GameObject go in Selection.gameObjects)
				go.transform.SetSiblingIndex (System.Array.IndexOf (selection, go));
		}
	}
}