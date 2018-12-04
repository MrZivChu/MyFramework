local MessageBoxPage
MessageBox = {}
function MessageBox.popOK( tip , callback , btnText )
    if MessageBoxPage == nil then
        MessageBoxPage = MF.route.push( 'MessageBox' )
    end
    MessageBoxPage:popOK(tip , callback , btnText)
end

function MessageBox.PopYesNo( tip , leftCallback, rightCallback , tleftBtnText ,trightBtnText)
    if MessageBoxPage == nil then
        MessageBoxPage = MF.route.push( 'MessageBox' )
    end
    MessageBoxPage:PopYesNo( tip , leftCallback, rightCallback , tleftBtnText ,trightBtnText)
end

