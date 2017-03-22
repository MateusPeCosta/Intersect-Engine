﻿using System;
using System.Collections.Generic;
using IntersectClientExtras.File_Management;
using IntersectClientExtras.Gwen;
using IntersectClientExtras.Gwen.Control;
using IntersectClientExtras.Gwen.Control.EventArguments;
using Intersect_Client.Classes.Core;
using Intersect_Client.Classes.General;
using Intersect_Client.Classes.Misc;
using Intersect_Client.Classes.Networking;
using Intersect_Library.GameObjects;
using Intersect_Library.Localization;
using Color = IntersectClientExtras.GenericClasses.Color;

namespace Intersect_Client.Classes.UI.Menu
{
    public class CreateCharacterWindow
    {
        //Controls
        private ImagePanel _menuPanel;
        private Label _menuHeader;

        private ImagePanel _characterNameBackground;
        private Label _charnameLabel;
        private TextBox _charnameTextbox;

        private ImagePanel _classBackground;
        private Label _classLabel;
        private ComboBox _classCombobox;

        private ImagePanel _genderBackground;
        private Label _genderLabel;
        private LabeledCheckBox _maleChk;
        private LabeledCheckBox _femaleChk;
        private Button _createButton;

        //Image
        private string _characterPortraitImg = "";
        private ImagePanel _characterPortrait;
        private ImagePanel _characterContainer;
        private Button _nextSpriteButton;
        private Button _prevSpriteButton;


        //Parent
        private MainMenu _mainMenu;

        //Class Info
        private List<KeyValuePair<int,ClassSprite>> _maleSprites = new List<KeyValuePair<int, ClassSprite>>();
        private List<KeyValuePair<int, ClassSprite>> _femaleSprites = new List<KeyValuePair<int, ClassSprite>>();
        private int _displaySpriteIndex = -1;

        //Init
        public CreateCharacterWindow(Canvas parent, MainMenu mainMenu, ImagePanel parentPanel)
        {
            //Assign References
            _mainMenu = mainMenu;

            //Main Menu Window
            _menuPanel = new ImagePanel(parent)
            {
                Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "uibody.png")
            };
            _menuPanel.SetSize(512, 393);
            if (_mainMenu != null && parentPanel != null)
            {
                _menuPanel.SetPosition(parentPanel.X, parentPanel.Y);
            }
            else
            {
                _menuPanel.SetPosition(parent.Width / 2 - _menuPanel.Width / 2, parent.Height / 2 - _menuPanel.Height / 2);
            }
            _menuPanel.IsHidden = true;

            //Menu Header
            _menuHeader = new Label(_menuPanel)
            {
                AutoSizeToContents = false
            };
            _menuHeader.SetText(Strings.Get("charactercreation","title"));
            _menuHeader.Font = Globals.ContentManager.GetFont(Gui.DefaultFont,24);
            _menuHeader.SetSize(_menuPanel.Width, _menuPanel.Height);
            _menuHeader.Alignment = Pos.CenterH;
            _menuHeader.TextColorOverride = new Color(255, 200, 200, 200);

            //Character Name Background
            _characterNameBackground = new ImagePanel(_menuPanel)
            {
                Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "inputfieldshort.png")
            };
            _characterNameBackground.SetSize(_characterNameBackground.Texture.GetWidth(), _characterNameBackground.Texture.GetHeight());
            _characterNameBackground.SetPosition(15, 44);

            //Character name Label
            _charnameLabel = new Label(_characterNameBackground);
            _charnameLabel.SetText(Strings.Get("charactercreation", "name"));
            _charnameLabel.AutoSizeToContents = false;
            _charnameLabel.SetSize(178, 60);
            _charnameLabel.Alignment = Pos.Center;
            _charnameLabel.TextColorOverride = new Color(255, 30, 30, 30);
            _charnameLabel.Font = Globals.ContentManager.GetFont(Gui.DefaultFont,20);

            //Character name Textbox
            _charnameTextbox = new TextBox(_characterNameBackground);
            _charnameTextbox.SubmitPressed += CharnameTextbox_SubmitPressed;
            _charnameTextbox.SetPosition(190, 8);
            _charnameTextbox.SetSize(188, 38);
            _charnameTextbox.ShouldDrawBackground = false;
            _charnameTextbox.TextColorOverride = new Color(255, 220, 220, 220);
            _charnameTextbox.Font = Globals.ContentManager.GetFont(Gui.DefaultFont, 20);

            //Class Background
            _classBackground = new ImagePanel(_menuPanel)
            {
                Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "inputfieldshort.png")
            };
            _classBackground.SetSize(_classBackground.Texture.GetWidth(), _classBackground.Texture.GetHeight());
            _classBackground.SetPosition(15, _characterNameBackground.Bottom + 16);

            //Class Label
            _classLabel = new Label(_classBackground);
            _classLabel.SetText(Strings.Get("charactercreation", "class"));
            _classLabel.AutoSizeToContents = false;
            _classLabel.SetSize(178, 60);
            _classLabel.Alignment = Pos.Center;
            _classLabel.TextColorOverride = new Color(255, 30, 30, 30);
            _classLabel.Font = Globals.ContentManager.GetFont(Gui.DefaultFont, 20);

            //Class Combobox
            _classCombobox = new ComboBox(_classBackground);
            var classCount = 0;
            foreach (var cls in ClassBase.GetObjects())
            {
                if (cls.Value.Locked == 0)
                {
                    _classCombobox.AddItem(cls.Value.Name);
                    classCount++;
                }
            }
            _classCombobox.ItemSelected += classCombobox_ItemSelected;
            _classCombobox.SetPosition(190, 8);
            _classCombobox.SetSize(200, 38);
            _classCombobox.Alignment = Pos.Center;
            _classCombobox.ShouldDrawBackground = false;
            _classCombobox.SetMenuBackgroundColor(new Color(220, 0, 0, 0));
            _classCombobox.SetMenuMaxSize(260, 200);
            _classCombobox.SetTextColor(new Color(255, 200, 200, 200), Label.ControlState.Normal);
            _classCombobox.SetTextColor(new Color(255, 220, 220, 220), Label.ControlState.Hovered);
            _classCombobox.Font = Globals.ContentManager.GetFont(Gui.DefaultFont, 20);

            //Character Container
            _characterContainer = new ImagePanel(_menuPanel);
            _characterContainer.SetSize(74, 74);
            _characterContainer.SetPosition(_menuPanel.Width -15 - _characterContainer.Width, 44);
            _characterContainer.Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui,"charview.png");

            //Character sprite
            _characterPortrait = new ImagePanel(_characterContainer);
            _characterPortrait.SetSize(48, 48);

            //Next Sprite Button
            _nextSpriteButton = new Button(_characterContainer);
            _nextSpriteButton.SetSize(15,15);
            _nextSpriteButton.SetPosition(74-15,_characterContainer.Height/2 - 15/2);
            _nextSpriteButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "rightarrownormal.png"),Button.ControlState.Normal);
            _nextSpriteButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "rightarrowhover.png"), Button.ControlState.Hovered);
            _nextSpriteButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "rightarrowclicked.png"), Button.ControlState.Clicked);
            _nextSpriteButton.Clicked += _nextSpriteButton_Clicked;

            //Prev Sprite Button
            _prevSpriteButton = new Button(_characterContainer);
            _prevSpriteButton.SetSize(15,15);
            _prevSpriteButton.SetPosition(0, _characterContainer.Height/2 - 15/2);
            _prevSpriteButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "leftarrownormal.png"), Button.ControlState.Normal);
            _prevSpriteButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "leftarrowhover.png"), Button.ControlState.Hovered);
            _prevSpriteButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "leftarrowclicked.png"), Button.ControlState.Clicked);
            _prevSpriteButton.Clicked += _prevSpriteButton_Clicked;

            //Class Background
            _genderBackground = new ImagePanel(_menuPanel)
            {
                Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "inputfield.png")
            };
            _genderBackground.SetSize(_genderBackground.Texture.GetWidth(), _genderBackground.Texture.GetHeight());
            _genderBackground.SetPosition(15, _classBackground.Bottom + 16);

            //Gender Label
            _genderLabel = new Label(_genderBackground);
            _genderLabel.SetText(Strings.Get("charactercreation", "gender"));
            _genderLabel.AutoSizeToContents = false;
            _genderLabel.SetSize(178, 60);
            _genderLabel.Alignment = Pos.Center;
            _genderLabel.TextColorOverride = new Color(255, 30, 30, 30);
            _genderLabel.Font = Globals.ContentManager.GetFont(Gui.DefaultFont, 20);

            //Male Checkbox
            _maleChk = new LabeledCheckBox(_genderBackground) { Text = Strings.Get("charactercreation", "male") };
            _maleChk.SetSize(200, 36);
            _maleChk.SetPosition(180, 8);
            _maleChk.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "checkboxempty.png"), CheckBox.ControlState.Normal);
            _maleChk.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "checkboxfull.png"), CheckBox.ControlState.CheckedNormal);
            _maleChk.SetCheckSize(32, 32);
            _maleChk.SetLabelDistance(8);
            _maleChk.SetTextColor(new Color(255, 200, 200, 200), Label.ControlState.Normal);
            _maleChk.SetTextColor(new Color(255, 140, 140, 140), Label.ControlState.Hovered);
            _maleChk.IsChecked = true;
            _maleChk.Checked += maleChk_Checked;
            _maleChk.UnChecked += femaleChk_Checked; // If you notice this, feel free to hate us ;)
            _maleChk.SetFont(Globals.ContentManager.GetFont(Gui.DefaultFont, 20));

            //Female Checkbox
            _femaleChk = new LabeledCheckBox(_genderBackground) { Text = Strings.Get("charactercreation", "female") };
            _femaleChk.Checked += femaleChk_Checked;
            _femaleChk.UnChecked += maleChk_Checked;
            _femaleChk.SetSize(200, 36);
            _femaleChk.SetPosition(300, 8);
            _femaleChk.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "checkboxempty.png"), CheckBox.ControlState.Normal);
            _femaleChk.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "checkboxfull.png"), CheckBox.ControlState.CheckedNormal);
            _femaleChk.SetCheckSize(32, 32);
            _femaleChk.SetLabelDistance(8);
            _femaleChk.SetTextColor(new Color(255, 200, 200, 200), Label.ControlState.Normal);
            _femaleChk.SetTextColor(new Color(255, 140, 140, 140), Label.ControlState.Hovered);
            _femaleChk.SetFont(Globals.ContentManager.GetFont(Gui.DefaultFont, 20));

            //Register - Send Registration Button
            _createButton = new Button(_menuPanel);
            _createButton.SetText(Strings.Get("charactercreation", "create"));
            _createButton.Clicked += CreateButton_Clicked;
            _createButton.SetSize(211, 61);
            _createButton.SetPosition(_menuPanel.Width/2 - _createButton.Width/2, _menuPanel.Height - 40 - _createButton.Height);
            _createButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "buttonnormal.png"), Button.ControlState.Normal);
            _createButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "buttonhover.png"), Button.ControlState.Hovered);
            _createButton.SetImage(Globals.ContentManager.GetTexture(GameContentManager.TextureType.Gui, "buttonclicked.png"), Button.ControlState.Clicked);
            _createButton.SetTextColor(new Color(255, 30, 30, 30), Label.ControlState.Normal);
            _createButton.SetTextColor(new Color(255, 20, 20, 20), Label.ControlState.Hovered);
            _createButton.SetTextColor(new Color(255, 215, 215, 215), Label.ControlState.Clicked);
            _createButton.Font = Globals.ContentManager.GetFont(Gui.DefaultFont, 20);

            LoadClass();
            Update();
        }

        //Methods
        public void Update()
        {
            var isFace = true;
            if (GetClass() != null && _displaySpriteIndex != -1)
            {
                _characterPortrait.IsHidden = false;
                if (GetClass().Sprites.Count > 0)
                {
                    if (_maleChk.IsChecked)
                    {
                        _characterPortrait.Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Face, _maleSprites[_displaySpriteIndex].Value.Face);
                        if (_characterPortrait.Texture == null)
                        {
                            _characterPortrait.Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Entity, _maleSprites[_displaySpriteIndex].Value.Sprite);
                            isFace = false;
                        }
                    }
                    else
                    {
                        _characterPortrait.Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Face, _femaleSprites[_displaySpriteIndex].Value.Face);
                        if (_characterPortrait.Texture == null)
                        {
                            _characterPortrait.Texture = Globals.ContentManager.GetTexture(GameContentManager.TextureType.Entity, _femaleSprites[_displaySpriteIndex].Value.Sprite);
                            isFace = false;
                        }
                    }
                    if (_characterPortrait.Texture != null)
                    {
                        if (isFace)
                        {
                            _characterPortrait.SetTextureRect(0, 0, _characterPortrait.Texture.GetWidth(),_characterPortrait.Texture.GetHeight());
                            _characterPortrait.SetSize(64, 64);
                            _characterPortrait.SetPosition(5,5);
                        }
                        else
                        {
                            _characterPortrait.SetTextureRect(0, 0, _characterPortrait.Texture.GetWidth() / 4, _characterPortrait.Texture.GetHeight() / 4);
                            _characterPortrait.SetSize(_characterPortrait.Texture.GetWidth() / 4,
                                _characterPortrait.Texture.GetHeight() / 4);
                            _characterPortrait.SetPosition(_characterContainer.Width / 2 - _characterPortrait.Width / 2,
                                _characterContainer.Height / 2 - _characterPortrait.Height / 2);
                        }
                    }
                }
            }
            else
            {
                _characterPortrait.IsHidden = true;
            }
        }

        public void Show()
        {
            _menuPanel.Show();
        }

        public void Hide()
        {
            _menuPanel.Hide();
        }

        private ClassBase GetClass()
        {
            if (_classCombobox.SelectedItem == null) return null;
            foreach (var cls in ClassBase.GetObjects())
            {
                if (_classCombobox.SelectedItem.Text == cls.Value.Name && cls.Value.Locked == 0)
                {
                    return cls.Value;
                }
            }
            return null;
        }

        private void LoadClass()
        {
            ClassBase cls = GetClass();
            _maleSprites.Clear();
            _femaleSprites.Clear();
            _displaySpriteIndex = -1;
            if (cls != null)
            {
                for (int i = 0; i < cls.Sprites.Count; i++)
                {
                    if (cls.Sprites[i].Gender == 0)
                    {
                        _maleSprites.Add(new KeyValuePair<int, ClassSprite>(i,cls.Sprites[i]));
                    }
                    else
                    {
                        _femaleSprites.Add(new KeyValuePair<int, ClassSprite>(i, cls.Sprites[i]));
                    }
                } 
            }
            ResetSprite();
        }

        private void ResetSprite()
        {
            _nextSpriteButton.IsHidden = true;
            _prevSpriteButton.IsHidden = true;
            if (_maleChk.IsChecked)
            {
                if (_maleSprites.Count > 0)
                {
                    _displaySpriteIndex = 0;
                    if (_maleSprites.Count > 1)
                    {
                        _nextSpriteButton.IsHidden = false;
                        _prevSpriteButton.IsHidden = false;
                    }
                }
                else
                {
                    _displaySpriteIndex = -1;
                }
            }
            else
            {
                if (_femaleSprites.Count > 0)
                {
                    _displaySpriteIndex = 0;
                    if (_femaleSprites.Count > 1)
                    {
                        _nextSpriteButton.IsHidden = false;
                        _prevSpriteButton.IsHidden = false;
                    }
                }
                else
                {
                    _displaySpriteIndex = -1;
                }
            }
        }

        private void _prevSpriteButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            _displaySpriteIndex--;
            if (_maleChk.IsChecked)
            {
                if (_maleSprites.Count > 0)
                {
                    if (_displaySpriteIndex == -1)
                    {
                        _displaySpriteIndex = _maleSprites.Count - 1;
                    }
                }
                else
                {
                    _displaySpriteIndex = -1;
                }
            }
            else
            {
                if (_femaleSprites.Count > 0)
                {
                    if (_displaySpriteIndex == -1)
                    {
                        _displaySpriteIndex = _femaleSprites.Count - 1;
                    }
                }
                else
                {
                    _displaySpriteIndex = -1;
                }
            }
            Update();
        }

        private void _nextSpriteButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            _displaySpriteIndex++;
            if (_maleChk.IsChecked)
            {
                if (_maleSprites.Count > 0)
                {
                    if (_displaySpriteIndex >= _maleSprites.Count)
                    {
                        _displaySpriteIndex = 0;
                    }
                }
                else
                {
                    _displaySpriteIndex = -1;
                }
            }
            else
            {
                if (_femaleSprites.Count > 0)
                {
                    if (_displaySpriteIndex >= _femaleSprites.Count)
                    {
                        _displaySpriteIndex = 0;
                    }
                }
                else
                {
                    _displaySpriteIndex = -1;
                }
            }
            Update();
        }

        void TryCreateCharacter(int Gender)
        {
            if (Globals.WaitingOnServer || _displaySpriteIndex == -1) { return; }
            if (FieldChecking.IsValidName(_charnameTextbox.Text))
            {
                GameFade.FadeOut();
                if (_maleChk.IsChecked)
                {
                    PacketSender.SendCreateCharacter(_charnameTextbox.Text, GetClass().Id, _maleSprites[_displaySpriteIndex].Key);
                }
                else
                {
                    PacketSender.SendCreateCharacter(_charnameTextbox.Text, GetClass().Id, _femaleSprites[_displaySpriteIndex].Key);
                }
                Globals.WaitingOnServer = true;
            }
            else
            {
                Gui.MsgboxErrors.Add(Strings.Get("charactercreation", "invalidname"));
            }
        }

        //Input Handlers
        void CharnameTextbox_SubmitPressed(Base sender, EventArgs arguments)
        {
            if (_maleChk.IsChecked == true)
            {
                TryCreateCharacter(0);
            }
            else
            {
                TryCreateCharacter(1);
            }
        }
        void classCombobox_ItemSelected(Base control, ItemSelectedEventArgs args)
        {
            LoadClass();
            Update();
        }
        void maleChk_Checked(Base sender, EventArgs arguments)
        {
            _maleChk.IsChecked = true;
            _femaleChk.IsChecked = false;
            ResetSprite();
            Update();
        }
        void femaleChk_Checked(Base sender, EventArgs arguments)
        {
            _femaleChk.IsChecked = true;
            _maleChk.IsChecked = false;
            ResetSprite();
            Update();
        }
        void CreateButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            if (_maleChk.IsChecked == true)
            {
                TryCreateCharacter(0);
            }
            else
            {
                TryCreateCharacter(1);
            }
        }
    }
}
