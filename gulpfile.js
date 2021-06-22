var gulp = require('gulp');
var run = require('gulp-run');
var argv = require('yargs').argv;

async function test() {
    console.log(argv.path);
    run('type nul > filename.txt').exec();
    return;
}

async function init() {
    let hubId = argv.hubid;
    let command = 'wchtools init --url https://content-eu-1.content-cms.com/api/' + hubId + ' --user adarsh.bhautoo@hangarww.com --password Ad1108bh_hangarMU';
    //run('wchtools init --url https://content-eu-1.content-cms.com/api/37dd7bf6-5628-4aac-8464-f4894ddfb8c4 --user adarsh.bhautoo@hangarww.com --password Ad1108bh_hangarMU').exec();
    run(command).exec();
    return;
}

async function pullAsset() {
    console.log("Starting assets pull");
    let assetPath = argv.path;
    let syncId = argv.syncid;
    let command = 'wchtools pull -a --dir artifacts/' + syncId + ' --path ' + assetPath + ' --user adarsh.bhautoo@hangarww.com --password Ad1108bh_hangarMU';
    //run('wchtools pull -a --dir artifacts --path /dxdam/fd/fda413f5-d9ac-406a-9360-13a626726837/KYO01145_KKI_Med_Comms_Website_Infographic_Modules_1_1388px_4_lo4d.png --user adarsh.bhautoo@hangarww.com --password Ad1108bh_hangarMU').exec();
    run(command).exec();
    console.log("End assets pull");
    return;
}

async function pushAssets() {
    let syncId = argv.syncid;
    let command = 'wchtools push -a -f --dir artifacts/' + syncId + ' -I --password Ad1108bh_hangarMU';
    run(command).exec();
    return;
}


async function pushContent() {
    let syncId = argv.syncid;
    let command = 'wchtools push -c -f --dir artifacts/' + syncId + ' -I --password Ad1108bh_hangarMU';
    run(command).exec();
    return;

}


var build = gulp.series(init, pullAsset, pushAssets, pushContent);
var pullAssetSeries = gulp.series(init, pullAsset);
var pushAssetSeries = gulp.series(init, pushAssets);
var pushContentSeries = gulp.series(init, pushContent);

exports.default = build;
exports.pullAssetJob = pullAssetSeries;
exports.pushAssetJob = pushAssetSeries;
exports.pushContentJob = pushContentSeries;

