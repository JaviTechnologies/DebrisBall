using UnityEngine;
using System.Collections;
using javitechnologies.ballwar.levelgenerator.spawner;

namespace javitechnologies.levelgenerator.editor
{
    public class SpawnerFactory
    {
        public static RandomSpawnerSetup CreateRandomSpawner()
        {
            RandomSpawnerSetup spawner = new RandomSpawnerSetup();

            return spawner;
        }
    }
}