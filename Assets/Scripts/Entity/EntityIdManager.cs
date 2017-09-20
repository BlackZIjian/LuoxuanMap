using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIdManager {
    private static EntityIdManager mInstance;
    public static EntityIdManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new EntityIdManager();
                mInstance.mEntityId = 0;
            }
            return mInstance;
        }
    }
    private int mEntityId;
    public int EntityId
    {
        get
        {
            mEntityId++;
            return mEntityId;
        }
    }
}
