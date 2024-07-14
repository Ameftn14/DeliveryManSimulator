using UnityEngine;
using System.Collections.Generic;

public class ColorDictionary {
    private static readonly Dictionary<int, Color> colorDictionary = new Dictionary<int, Color>()
    {
        {0, new Color(0.039f, 0.518f, 1f)},     // 蓝色
        {1, new Color(1f, 0.839f, 0.039f)},     // 黄色
        {2, new Color(0.75f, 0.353f, 0.95f)},   // 紫色
        {3, new Color(1f, 0.624f, 0.039f)},     // 橙色
        {4, new Color(0.196f, 0.843f, 0.294f)}, // 绿色
        {5, new Color(1f, 0f, 1f)},             // 品红
        {6, new Color(0f, 1f, 0.5f)},           // 酸橙色
        {7, new Color(1f, 0.5f, 0.5f)},         // 粉红
        {8, new Color(0f, 0.5f, 0.5f)},         // 蓝绿色
        {9, new Color(1f, 0.84f, 0f)},          // 金色
        {10, new Color(0.53f, 0.81f, 0.92f)},   // 天蓝色
        {11, new Color(0.5f, 0f, 0f)},          // 栗色
        {12, new Color(0.5f, 0.5f, 0f)}         // 橄榄色
    };

    public static Color GetColor(int index) {
        if (colorDictionary.Count == 0) {
            Debug.LogError("Color dictionary is empty!");
            return Color.black;
        }

        // 取模运算确保索引在有效范围内循环
        int modIndex = Mathf.Abs(index % colorDictionary.Count);

        // 使用取模后的索引获取颜色
        return colorDictionary[modIndex];
    }
}
