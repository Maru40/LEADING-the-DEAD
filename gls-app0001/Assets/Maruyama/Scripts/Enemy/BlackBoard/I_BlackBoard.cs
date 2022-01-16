using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BlackBoard<StructType>
    where StructType : struct
{
    public ref StructType Struct { get; }

    public StructType GetStruct();
}
