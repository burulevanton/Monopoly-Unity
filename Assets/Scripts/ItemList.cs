using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour {
    
    public ScrollRect scroll;
    public RectTransform element; // кнопка из которой будет составлен список
    public int offset = 10; // расстояние между элементами
    
    private Action<Ownable> _action;
    private Vector2 delta;
    private Vector2 e_Pos;
    private List<RectTransform> buttons;
    private int size;
    private float curY, vPos;
    
    void Awake()
    {
        buttons = new List<RectTransform>();
        delta = element.sizeDelta;
        delta.y += offset;
        e_Pos = new Vector2(0, -delta.y / 2);
    }

    public void SetAction(Action<Ownable> action)
    {
        _action = action;
    }
    void ButtonPressed(Ownable property)
    {
        UIManager uiManager = this.GetComponentInParent<UIManager>();
        _action(property);
        UpdateList(property);
    }
    void UpdateList(Ownable property) // функция удаления элемента
    {
        vPos = scroll.verticalNormalizedPosition; // запоминаем позицию скролла
        int j = 0;
        var item = buttons.Find(x => x.GetComponent<ItemButton>().property == property);
        Destroy(item.GetComponent<ItemButton>().gameObject); // удаляем этот элемент из списка
        buttons.Remove(item); // удаляем этот элемент из массива
        curY = 0;
        size--; // минус один элемент
        RectContent(); // пересчитываем размеры окна
        foreach(RectTransform b in buttons) // сдвигаем элементы
        {
            b.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
            curY += delta.y;
        }
        scroll.verticalNormalizedPosition = vPos; // возвращаем позицию скролла
    }
    public void AddToList(Ownable property, bool resetScrollbar)
    {
        element.gameObject.SetActive(true);
        vPos = scroll.verticalNormalizedPosition;
        curY = 0;
        size++;
        RectContent();
        foreach(RectTransform b in buttons)
        {
            b.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
            curY += delta.y;
        }
        BuildElement(property);
        if(!resetScrollbar) scroll.verticalNormalizedPosition = vPos;
        element.gameObject.SetActive(false);
    }

    void BuildElement(Ownable property) // создание нового элемента и настройка параметров
    {
        RectTransform clone = Instantiate(element) as RectTransform;
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        ItemButton item = clone.GetComponent<ItemButton>();
        item.mainButtonText.text = property.name;
        item.property = property;
        item.mainButton.onClick.AddListener(() => ButtonPressed(property));
        buttons.Add(clone);
    }

    void RectContent() // определение размера окна с элементами
    {
        float height = delta.y * size;
        scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, height);
        scroll.content.anchoredPosition = Vector2.zero;
    }

    public void Clear()
    {
        foreach(RectTransform b in buttons)
        {
            Destroy(b.gameObject);
        }
        buttons = new List<RectTransform>();
    }
}
