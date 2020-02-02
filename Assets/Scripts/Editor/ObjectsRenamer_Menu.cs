namespace RSTools
{
	using UnityEditor;

	sealed class ObjectsRenamer_Menu
	{
		const string SHORTCUT = "%&r";

		[MenuItem ("GameObject/Rename Objects " + SHORTCUT, true)]
		static bool CheckIfAtLeastOneObjectIsSelect () => Selection.gameObjects.Length > 0;

		[MenuItem ("GameObject/Rename Objects " + SHORTCUT)]
		public static void RenameSelectedObjects () => ObjectsRenamer_Editor.LaunchRenamer ();
	}
}