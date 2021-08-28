using UnityEngine;


    public class NotificationEventArgs
    {
        public GameObject publisher;
    }

    public class EnemyDeathNotificationEventArgs : NotificationEventArgs
    {
        public Enemy enemy;

        public EnemyDeathNotificationEventArgs(Enemy enemy)
        {
            this.publisher = enemy.gameObject;
            this.enemy = enemy;
        }
    }

    public class LootRequestNotificationEventArgs : NotificationEventArgs
    {
        public LootTableData lootTableData;

        public LootRequestNotificationEventArgs(LootTableData lootTableData, GameObject publisher)
        {
            this.publisher = publisher;
            this.lootTableData = lootTableData;
        }
    }
