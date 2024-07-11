using UnityEngine;
using System.Collections.Generic;

public class ColorDictionary {
    private readonly Dictionary<int, Color> colorDictionary = new Dictionary<int, Color>()
    {
        {0, new Color(1f, 0f, 0f)},             // 红色
        {1, new Color(0f, 0f, 1f)},             // 蓝色
        {2, new Color(0.7f, 0.7f, 0.3f)},       // 黄色
        {3, new Color(0.5f, 0f, 0.5f)},         // 紫色
        {4, new Color(1f, 0.5f, 0f)},           // 橙色
        {5, new Color(0f, 1f, 1f)},             // 青色
        {6, new Color(1f, 0f, 1f)},             // 品红
        {7, new Color(0f, 1f, 0.5f)},           // 酸橙色
        {8, new Color(1f, 0.5f, 0.5f)},         // 粉红
        {9, new Color(0f, 0.5f, 0.5f)},        // 蓝绿色
        {10, new Color(1f, 0.84f, 0f)},         // 金色
        {11, new Color(0.53f, 0.81f, 0.92f)},   // 天蓝色
        {12, new Color(0.5f, 0f, 0f)},          // 栗色
        {13, new Color(0.5f, 0.5f, 0f)}         // 橄榄色
    };

    public Color GetColor(int index) {
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
