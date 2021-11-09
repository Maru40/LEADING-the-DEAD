using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace MaruUtility
{
    namespace UtilityDictionary
    {

        /// <summary>
        /// DictionaryをInspectorで表示したいための拡張、
        /// AwakeかStartでInsertInspectorData関数を呼んでください。
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        [Serializable]
        public class Ex_Dictionary<TKey, TValue> : Dictionary<TKey, TValue>
        {
            [Serializable]
            struct TypeDraw
            {
                public TKey key;
                public TValue value;
            }

            [SerializeField]
            List<TypeDraw> m_typeDraws = new List<TypeDraw>();

            public Ex_Dictionary()
                : base()
            { }

            /// <summary>
            /// Inspectorに表示していた情報を挿入する処理
            /// </summary>
            public void InsertInspectorData()
            {
                foreach (var type in m_typeDraws)
                {
                    base.Add(type.key, type.value);
                }
                m_typeDraws.Clear();
            }
        }
    }
}


