using UnityEngine;


    public class NotificationEventArgs
    {
        public GameObject publisher;
    }

public class EnemyHitNotificationEventArgs : NotificationEventArgs
{
    public Enemy enemy;
    public GameObject hitter;

    public EnemyHitNotificationEventArgs(Enemy enemy, GameObject hitter)
    {
        this.publisher = enemy.gameObject;
        this.enemy = enemy;
        this.hitter = hitter;
    }
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
