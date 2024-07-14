using System.Numerics;

interface ISkillSlot {
    Vector2 getNextSkillItemEndPosition();
    void addItem(IPurchacedItem item);
}