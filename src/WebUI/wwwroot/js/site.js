const Counter = {
    data() {
        return {
            siteUrl: 'https://www.simbirsoft.com/',
            words: null,
            showError: false,
            errorText: '',
        }
    },

    methods: {
        parse: async function(){

            this.words = null;
            
            let response = await fetch("api/parser",{
                method: "post",
                headers: { 'Content-type': 'application/json' },
                body: JSON.stringify({
                    url: this.siteUrl
                })
            });

            let json = await response.json();
            
            if (!response.ok){
                this.showError = true;
                this.errorText = json.errors.Url[0];
            }
            else {
                this.words = json.items;
            }
        },

        hideError: function(){
            this.showError = false;
            this.errorText = '';
        }
    }
}

Vue.createApp(Counter).mount('#container')