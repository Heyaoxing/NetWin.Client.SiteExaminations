function mSift_SeekTp(oObj, nDire) {
    console.log(oObj.offsetLeft);
    console.log(oObj.offsetWidth);
    console.log(oObj.offsetHeight);
    console.log(oObj.getBoundingClientRect().top);
    console.log(oObj.getBoundingClientRect().right);
    console.log(oObj.getBoundingClientRect().left);

    if (oObj.getBoundingClientRect && !document.all) {
        var oDc = document.documentElement; switch (nDire) {
            case 0: return oObj.getBoundingClientRect().top + oDc.scrollTop;
            case 1: return oObj.getBoundingClientRect().right + oDc.scrollLeft;
            case 2: return oObj.offsetHeight * 2; ;
            case 3: return oObj.getBoundingClientRect().left + oDc.scrollLeft;
        }
    }
    else {
        if (nDire == 1 || nDire == 3) {
            var nPosition = oObj.offsetLeft;

        }
        else { var nPosition = oObj.offsetTop; } if (arguments[arguments.length - 1] != 0) {
            if (nDire == 1) {
                nPosition += oObj.offsetWidth;

            }
            else if (nDire == 2) {
                nPosition += oObj.offsetHeight;

            }
        } if (oObj.offsetParent != null) {
            nPosition += mSift_SeekTp(oObj.offsetParent, nDire, 0);

        }
        return nPosition;
    }
}

function mSift(cVarName, nMax) { this.oo = cVarName; this.Max = nMax; }
mSift.prototype = {
    Varsion: 'v2010.10.29',
    Target: Object,
    TgList: Object,
    Listeners: null,
    SelIndex: 0,
    Data: [],
    ReData: [],
    Create: function (oObj) {
        var _this = this;
        var oUL = document.createElement('ul');
        oUL.style.display = 'none';

        oObj.parentNode.insertBefore(oUL, oObj);
        _this.TgList = oUL;
        oObj.onkeydown = oObj.onclick = function (e) {
            _this.Listen(this, e);
        };
        oObj.onblur = function () { setTimeout(function () { _this.Clear(); }, 100); };
    },
    Complete: function () {
    },
    Select: function () {
        var _this = this;
        if (_this.ReData.length > 0) {
            _this.Target.value = _this.ReData[_this.SelIndex].SiteUrl.replace(/\*/g, '*').replace(/\|/g, '|');
            _this.Clear();
        }
        setTimeout(function () { _this.Target.focus(); }, 10);
        _this.Complete();
    },
    Listen: function (oObj) {
        var _this = this;
        _this.Target = oObj;
        var e = arguments[arguments.length - 1];
        var ev = window.event || e;
        switch (ev.keyCode) {
            case 9: //TAB
                return;
            case 13: //ENTER
                _this.Target.blur();
                _this.Select();
                document.getElementById("submit_id").click();
                return;
            case 38: //UP
                _this.SelIndex = _this.SelIndex > 0 ? _this.SelIndex - 1 : _this.ReData.length - 1;
                break;
            case 40: //DOWN
                _this.SelIndex = _this.SelIndex < _this.ReData.length - 1 ? _this.SelIndex + 1 : 0;
                break;
            default:
                _this.SelIndex = 0;
        }
        if (_this.Listeners) { clearInterval(_this.Listeners); }
        _this.Listeners = setInterval(function () {
            _this.Get();
        }, 10);
    },
    Get: function () {
        var _this = this;
        //调用历史记录
        var siteurl = "";
        if (typeof (_this.Target.value) != 'undefined') {
            siteurl = _this.Target.value;
        }
        //   var historyjson = jQuery.parseJSON(window.external.GetHistories(siteurl));
        if (typeof (siteurl) == 'undefined'|| siteurl=="") {
            siteurl = "http";
        }
        var historyjson = jQuery.parseJSON(window.CallCSharpMethod("GetHistories", ""+siteurl));
   
        _this.Data = historyjson;


        if (_this.Target.value == '') { _this.Clear(); return; }
        if (_this.Listeners) { clearInterval(_this.Listeners); };
        _this.ReData = [];
        var cResult = '';
        for (var i = 0; i < _this.Data.length; i++) {
            if (_this.Data[i].SiteUrl.toLowerCase().indexOf(_this.Target.value.toLowerCase()) >= 0) {
                _this.ReData.push(_this.Data[i]);
                if (_this.ReData.length == _this.Max) { break; }
            }
        }
        var cRegPattern = _this.Target.value.replace(/\*/g, '*');
        cRegPattern = cRegPattern.replace(/\|/g, '|');
        cRegPattern = cRegPattern.replace(/\+/g, '\\+');
        cRegPattern = cRegPattern.replace(/\./g, '\\.');
        cRegPattern = cRegPattern.replace(/\?/g, '\\?');
        cRegPattern = cRegPattern.replace(/\^/g, '\\^');
        cRegPattern = cRegPattern.replace(/\$/g, '\\$');
        cRegPattern = cRegPattern.replace(/\(/g, '\\(');
        cRegPattern = cRegPattern.replace(/\)/g, '\\)');
        cRegPattern = cRegPattern.replace(/\[/g, '\\[');
        cRegPattern = cRegPattern.replace(/\]/g, '\\]');
        cRegPattern = cRegPattern.replace(/\\/g, '\\\\');
        var cRegEx = new RegExp(cRegPattern, 'i');
        for (var i = 0; i < _this.ReData.length; i++) {
            if (_this.Target.value.indexOf('*') >= 0) {
                _this.ReData[i].SiteUrl = _this.ReData[i].SiteUrl.replace(/\*/g, '*');
            }
            if (_this.Target.value.indexOf('|') >= 0) {
                _this.ReData[i].SiteUrl = _this.ReData[i].SiteUrl.replace(/\|/g, '|');
            }
            cResult += '<li style="padding:0 5px;line-height:20px;cursor:default; color:#222; text-align: left; " onmouseover="' +
_this.oo + '.ChangeOn(this);' + _this.oo + '.SelIndex=' + i + ';" ><span onmousedown="' + _this.oo + '.Select();">'
+ _this.ReData[i].SiteUrl.replace(cRegEx, function (s) { return '<span style="background:#ff9;font-weight:bold;font-style:normal;color:#e60;">' + s + '</span>'; }) + '<br/>上次体检时间:' + _this.ReData[i].CompletedOnString + '</span><a style="cursor:pointer;float:right;width:200px;font-size:18px;" onclick="SelectHistory(' + _this.ReData[i].SiteId + ')" alt="'  +_this.ReData[i].SiteId  +'">&nbsp;&nbsp;查看历史记录&nbsp;&nbsp;</a>' + '</li>';
        }
        if (cResult == '') { _this.Clear(); }
        else {
            _this.TgList.innerHTML = cResult;
            _this.TgList.style.cssText = 'display:block;position:absolute;background:#fff;border:#666 solid 1px;margin:-1px 0 0;padding: 5px;list-style:none;font-size:12px;';
            _this.TgList.style.top = _this.Target.offsetTop + _this.Target.offsetHeight + 'px';
            _this.TgList.style.left = _this.Target.offsetLeft + 'px';
            _this.TgList.style.width = _this.Target.offsetWidth + 'px';
        }
        var oLi = _this.TgList.getElementsByTagName('li');
        if (oLi.length > 0) {
            oLi[_this.SelIndex].style.cssText = 'background:#36c;padding:0 5px;line-height:20px;cursor:default;color:#222;text-align: left;';
        }
    },
    ChangeOn: function (oObj) {
        var oLi = this.TgList.getElementsByTagName('li');
        for (var i = 0; i < oLi.length; i++) {
            oLi[i].style.cssText = 'padding:0 5px;line-height:20px;cursor:default;color:#222;text-align: left;';
        }
        oObj.style.cssText = 'background:#36c;padding:0 5px;line-height:20px;cursor:default;color:#222;text-align: left;';
    },
    Clear: function () {
        var _this = this;
        if (_this.TgList) {
            _this.TgList.style.display = 'none';
            _this.ReData = [];
            _this.SelIndex = 0;
        }
    }
}

