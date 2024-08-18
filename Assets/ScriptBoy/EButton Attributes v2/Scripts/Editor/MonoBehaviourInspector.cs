using UnityEngine;
using UnityEditor;

namespace ScriptBoy.EButtonEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    class MonoBehaviourInspector : Editor
    {
        public EButtonDrawer m_EButtonDrawer;

        public void OnEnable()
        {
            m_EButtonDrawer = new EButtonDrawer(targets);
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            m_EButtonDrawer.Draw();
        }
    }
}