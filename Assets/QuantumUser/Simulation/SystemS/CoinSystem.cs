namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine;
    using UnityEngine.Scripting;

    [Preserve]
    public unsafe class CoinSystem : SystemMainThread, ISignalOnTriggerEnter3D
    {
        private EntityRef _lastCoinEntity;
        private bool _coinSpawned = false;

        public override void Update(Frame f)
        {
            // ตรวจสอบว่าเป็นเฟรมที่ 200 หรือยัง
            if (!_coinSpawned && f.Number >= 200)
            {
                if (f.IsVerified)
                {
                    SpawnCoin(f);
                    _coinSpawned = true; // กำหนดค่าว่าเหรียญถูกสร้างแล้ว
                }
            }
        }

        private void SpawnCoin(Frame f)
        {
            Debug.Log("Fnumber " + f.Number);
            // หา CoinSpawnPointData สำหรับการสุ่มตำแหน่ง
            CoinSpawnPointData spawnPointDataAsset = f.FindAsset<CoinSpawnPointData>(f.Map.UserAsset.Id);

            FPVector3 coinPosition = new FPVector3(1,2,1);

            
            EntityRef coinEntity = f.Create(f.SimulationConfig.Coin);


            f.Set<Transform3D>(coinEntity, new Transform3D
            {
                Position = coinPosition,
                Rotation = FPQuaternion.Identity
               
            });

            // บันทึกค่า coinEntity (หากต้องการใช้ภายหลัง)
            _lastCoinEntity = coinEntity;

            // ทำเครื่องหมายว่าเหรียญถูกสร้างแล้ว
            _coinSpawned = true;
            Debug.Log("Fnumber " + _coinSpawned);

        }

        // ตรวจสอบการชน (trigger) เพื่อทำให้เหรียญหายเมื่อผู้เล่นเก็บเหรียญ
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
            Debug.Log("  the coin!");
            // ตรวจสอบว่าผู้เล่นชนกับเหรียญหรือไม่
            /*if (f.Has<PlayerLink>(info.Entity) && f.Has<Coin>(info.Other))
            {
                // ทำลายเหรียญเมื่อผู้เล่นเก็บ
                f.Destroy(info.Other);
                Debug.Log("Player collected the coin!");

                // กำหนดค่าให้สามารถสร้างเหรียญใหม่ได้ในอนาคต
                _coinSpawned = false;
                _lastCoinEntity = default;
            }*/
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public Coin* Coin;
        }
    }
}
