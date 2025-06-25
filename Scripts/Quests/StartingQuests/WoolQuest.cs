using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WoolQuest : BaseQuest
    {
        public WoolQuest()
        {
            AddObjective(new SlayObjective(typeof(Sheep), "Sheep", 10));
            AddObjective(new ObtainObjective(typeof(Wool), "Wool", 5, 0xDF8));

            AddReward(new BaseReward(typeof(WoolQuestRewardBag), "Reward Bag"));
        }

        public override object Title => 1094949;

        public override object Description => 1094951;

        public override object Refuse => 1094952;

        public override object Uncomplete => 1094953;

        public override object Complete => 1094956;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
