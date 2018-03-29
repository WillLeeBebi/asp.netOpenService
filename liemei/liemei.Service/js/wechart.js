var appId = "";
var timeStamp = "";
var nonceStr = "";
var pg = "";
var signType = "";
var paySign = "";
var orderid = "";
/*
    *psid 测评id ,
    userid测评所有者ID
    useropenid当前用户openid
    */
function WeChartBuyPtest(psid, userid, useropenid) {
    $.ajax({
        type: "post",
        url: "/api/Pay/UnifiedOrder",
        contentType: 'application/json',
        data: JSON.stringify({ PayTyp: '1', product_id: psid, productType: '1', trade_type: 'JSAPI', openid: useropenid, userid: userid }),
        success: function (result, status) {
            console.log(result);
            if (result.code == 1) {
                orderid = result.Result.ID;

                $.get("/api/Pay/jssign?prepay_id=" + result.Result.prepay_id, function (data) {
                    console.log(data);
                    if (data.code == 1) {
                        //appId = data.Result.appid;
                        //timeStamp = data.Result.timestamp;
                        //nonceStr = data.Result.nonceStr;
                        //pg = data.Result.package;
                        //signType = data.Result.signType;
                        //paySign = data.Result.paySign;

                        wx.chooseWXPay({
                            timestamp: data.Result.timestamp,
                            nonceStr: data.Result.nonceStr,
                            package: "prepay_id="+data.Result.package, // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
                            signType: data.Result.signType, // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
                            paySign: data.Result.paySign, // 支付签名
                            success: function (res) {
                                var _send = setInterval(function () {
                                    $.get("/Psychtest/PsychTestPayState?id=" + orderid + "&userid=" + userid + "&pid=" + psid, function (state) {
                                        console.log(state);
                                        if (state.code == "OK") {
                                            window.clearInterval(_send);
                                            location.href = '/Psychtest/PsychTestStart?id=' + state.id
                                        } else if (state.code == "Fail") {
                                            window.clearInterval(_send);
                                            alter("支付失败，请重新购买");
                                        }
                                    });
                                    i++;
                                    if (i > 60) {
                                        window.clearInterval(_send);
                                        alter("支付失败，请重新购买");
                                    }
                                }, 500);
                                
                            }
                        });

                    }
                }, "json");
            }
        }
    });
    //唤起微信支付  
    function pay() {
        if (typeof WeixinJSBridge == "undefined") {
            if (document.addEventListener) {
                document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
            } else if (document.attachEvent) {
                document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
                document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
            }
        } else {
            onBridgeReady();
        }

    }
    //开始支付  
    function onBridgeReady() {
        WeixinJSBridge.invoke(
            'getBrandWCPayRequest', {
                "appId": appId,     //公众号名称，由商户传入         
                "timeStamp": timeStamp + "",         //时间戳，自1970年以来的秒数         
                "nonceStr": nonceStr, //随机串         
                "package": "prepay_id=" + pg,
                "signType": signType,         //微信签名方式:         
                "paySign": paySign    //微信签名     
            },

            function (res) {
                if (res.err_msg == "get_brand_wcpay_request:ok") {
                    var i = 0;
                            var _send = setInterval(function () {
                                $.get("/Psychtest/PsychTestPayState?id=" + result.Result.ID + "&userid=" + userid + "&pid=" + psid, function (state) {
                                    console.log(state);
                                    if (state.code == "OK") {
                                        window.clearInterval(_send);
                                        location.href = '/Psychtest/PsychTestStart?id=' + state.id
                                        } else if (state.code == "Fail") {
                                        window.clearInterval(_send);
                                        alter("支付失败，请重新购买");
                                    }
                                });
                                i++;
                                if (i > 60) {
                                    window.clearInterval(_send);
                                    alter("支付失败，请重新购买");
                                }
                            }, 1000);
                } else if (res.err_msg == "get_brand_wcpay_request:cancel") {
                    alert("支付过程中用户取消");
                } else {
                    //支付失败  
                    alert(res.err_msg)
                }
            }
        );
    }
        //$.post("/api/Pay/UnifiedOrder", JSON.stringify({ PayTyp: 1, product_id: psid, productType: 1, trade_type: 'JSAPI', openid: useropenid }), function (result) {
        //        console.log(result);
        //        if(result.code==1){
        //            $.get("/api/Pay?prepay_id="+result.Result.prepay_id,function(data){
        //                console.log(data);
        //                if(data.code==1){
        //                    wx.chooseWXPay({
        //                        timestamp: data.timestamp,
        //                        nonceStr:data.nonceStr,
        //                        package: data.package, // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
        //                        signType: data.signType, // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
        //                        paySign: data.paySign, // 支付签名
        //                        success: function (res) {
        //                            var i = 0;
        //                            var _send = setInterval(function(){
        //                                $.get("/Psychtest/PsychTestPayState?id=" + result.Result.ID + "&userid=" + userid + "&pid=" + psid, function (state) {
        //                                    if(state=="OK"){
        //                                        window.clearInterval(_send);
        //                                        location.href='/Psychtest/PsychTestStart?id='+psid
        //                                    }else if(state=="Fail"){
        //                                        window.clearInterval(_send);
        //                                        alter("支付失败，请重新购买");
        //                                    }
        //                                });
        //                                i++;
        //                                if(i>60){
        //                                    window.clearInterval(_send);
        //                                    alter("支付失败，请重新购买");
        //                                }
        //                            },1000);
        //                        }
        //                    });
        //                }
        //            },"json");
        //        }
        //    },"json");
    }

    var wecharPsychtestConfig = {
        Name: "",
        imgUrl: "",
        desc: "",
        //测评ID
        pid: "",
        //当前用户ID
        louserid: "",
        //所有者ID
        userid: "",
        //微信配置初始化
        WeiXinJsapiConfig: function () {
            var url = encodeURIComponent(location.href);
            console.log(this.Name);
            console.log(this.louserid);
            console.log(this.desc);
            var link = 'http://xxxx?id=' + this.pid + "&userid=" + this.louserid
            $.get("/WeixinRedirect/jssignature?url=" + url, function (data) {
                wx.config({
                    debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                    appId: data['appid'], // 必填，公众号的唯一标识
                    timestamp: data['timestamp'], // 必填，生成签名的时间戳
                    nonceStr: data['noncestr'], // 必填，生成签名的随机串
                    signature: data['signature'], // 必填，签名，见附录1
                    jsApiList: ['onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo', 'onMenuShareQZone', 'chooseWXPay'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                });
                wx.ready(function () {
                    //分享到朋友圈
                    wx.onMenuShareTimeline({
                        title: Name, // 分享标题
                        link: this.link, // 分享链接
                        imgUrl: this.imgUrl, // 分享图标
                        success: function () {
                            // 用户确认分享后执行的回调函数
                           
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                    //分享给朋友
                    wx.onMenuShareAppMessage({
                        title: Name, // 分享标题
                        desc: this.desc, // 分享描述
                        link: this.link, // 分享链接
                        imgUrl: this.imgUrl, // 分享图标
                        type: '', // 分享类型,music、video或link，不填默认为link
                        dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空,
                        success: function () {
                            // 用户确认分享后执行的回调函数
                            console.log(this.pid);
                            console.log(louserid);
                           
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                    //分享到QQ
                    wx.onMenuShareQQ({
                        title: Name, // 分享标题
                        desc: this.desc, // 分享描述
                        link: this.link, // 分享链接
                        imgUrl: this.imgUrl, // 分享图标
                        success: function () {
                            // 用户确认分享后执行的回调函数
                           
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                    //分享到腾讯微博
                    wx.onMenuShareWeibo({
                        title: Name, // 分享标题
                        desc: this.desc, // 分享描述
                        link: this.link, // 分享链接
                        imgUrl: this.imgUrl, // 分享图标
                        success: function () {
                            // 用户确认分享后执行的回调函数
                           
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                    //分享到QQ空间
                    wx.onMenuShareQZone({
                        title: Name, // 分享标题
                        desc: this.desc, // 分享描述
                        link: this.link, // 分享链接
                        imgUrl: this.imgUrl, // 分享图标
                        success: function () {
                            // 用户确认分享后执行的回调函数
                           
                        },
                        cancel: function () {
                            // 用户取消分享后执行的回调函数
                        }
                    });
                });
                wx.error(function (res) {
                    // alert("err" + res);
                });
            });
        }
    }